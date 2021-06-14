--views 
IF OBJECT_ID('groupHrsJoin') IS NOT NULL
	DROP VIEW groupHrsJoin
GO
CREATE VIEW groupHrsJoin AS
	SELECT HORARIO_GRUPO.grupo_fk, HORARIO.hora,HORARIO.lunes,HORARIO.martes,HORARIO.miercoles,HORARIO.jueves,HORARIO.viernes,HORARIO.sabado, GRUPO.aula_fk,GRUPO.maestro_fk,GRUPO.semestre_fk FROM 
		   HORARIO INNER JOIN HORARIO_GRUPO ON HORARIO.id = HORARIO_GRUPO.horario_fk INNER JOIN GRUPO ON GRUPO.id = HORARIO_GRUPO.grupo_fk
GO

IF OBJECT_ID('studentsInfo') IS NOT NULL
	DROP VIEW studentsInfo
GO
CREATE VIEW studentsInfo AS
	SELECT	ALUMNO.matricula AS Matricula,dbo.f_fullName(ALUMNO.apellido_Pat,ALUMNO.apellido_Mat,ALUMNO.nombre) AS 'Nombre_Completo',
					ALUMNO.nombre AS Nombre,ALUMNO.apellido_Pat AS 'Apellido_Paterno',ALUMNO.apellido_Mat AS 'Apellido_Materno',
					CARRERA.alias AS Carrera, CASE WHEN ALUMNO.activo = 1 THEN 'Activo' ELSE 'Inactivo' END AS Estado,
					ALUMNO.genero AS Genero, PAIS.nombre AS Pais, ALUMNO.fecha_Naci AS Nacimiento, YEAR(GETDATE())-YEAR(ALUMNO.fecha_Naci) AS Edad
					FROM ALUMNO INNER JOIN CARRERA ON ALUMNO.carrera_fk = CARRERA.clave INNER JOIN PAIS ON ALUMNO.pais_fk = PAIS.id
GO

IF OBJECT_ID('groupsPerCareer') IS NOT NULL
	DROP VIEW groupsPerCareer
GO
CREATE VIEW groupsPerCareer AS
	SELECT	GRUPO.id Grupo, MATERIA_CARRERA.carrera_fk Carrera, GRUPO.materia_fk Materia, BLOQUEO_MATERIA.bloque_fk Bloqueo
			FROM GRUPO INNER JOIN MATERIA_CARRERA ON GRUPO.materia_fk = MATERIA_CARRERA.materia_fk
			LEFT JOIN BLOQUEO_MATERIA ON GRUPO.materia_fk = BLOQUEO_MATERIA.materia_fk
			INNER JOIN dbo.thisSemester ON GRUPO.semestre_fk = dbo.thisSemester.id
GO

IF OBJECT_ID('nextSemester') IS NOT NULL
	DROP VIEW nextSemester
GO
CREATE VIEW nextSemester AS
	SELECT TOP 1	SEMESTRE.id,SEMESTRE.estado,SEMESTRE.fecha_Inicio FROM SEMESTRE WHERE DATEDIFF(DAY,GETDATE(),SEMESTRE.fecha_Inicio) > 0
					ORDER BY SEMESTRE.fecha_Inicio ASC
GO

IF OBJECT_ID('thisSemester') IS NOT NULL
	DROP VIEW thisSemester
GO
CREATE VIEW thisSemester AS
	SELECT SEMESTRE.id FROM SEMESTRE WHERE SEMESTRE.estado = 1;
GO

--functions

IF OBJECT_ID('isStudentAproved') IS NOT NULL
	DROP FUNCTION isStudentAproved
GO
CREATE FUNCTION isStudentAproved(@student INT, @subject INT)
RETURNS FLOAT AS
BEGIN
	IF @subject IS NULL
		RETURN 1.0;
	IF EXISTS (SELECT	MATERIA.nombre FROM MATERIA INNER JOIN groupsPerCareer ON MATERIA.clave = groupsPerCareer.Materia
						INNER JOIN ALUMNO_GRUPO ON groupsPerCareer.Grupo = ALUMNO_GRUPO.grupo_fk
						INNER JOIN ALUMNO ON ALUMNO_GRUPO.alumno_fk = ALUMNO.matricula
						WHERE ALUMNO_GRUPO.alumno_fk = @student AND ALUMNO_GRUPO.calificacion > 70.0 
						AND MATERIA.clave = @subject AND ALUMNO.activo = 1)
	BEGIN
		RETURN 1.0;
	END
	RETURN 0.0;
END
GO

IF OBJECT_ID('semesterName') IS NOT NULL
	DROP FUNCTION semesterName
GO
CREATE FUNCTION semesterName(@semester INT)
RETURNS VARCHAR(25) AS
BEGIN 
	DECLARE @result VARCHAR(25);
	SELECT @result = CONCAT(DATENAME(Month,SEMESTRE.fecha_Inicio),'-',DATENAME(Month,ISNULL(SEMESTRE.fecha_fin,GETDATE())),'-',CONVERT(VARCHAR,YEAR(SEMESTRE.fecha_fin),25)) FROM SEMESTRE WHERE SEMESTRE.id = @semester;
	RETURN @result;
END
GO

IF OBJECT_ID('f_fullName') IS NOT NULL
	DROP FUNCTION f_fullName
GO
CREATE FUNCTION f_fullName (@name VARCHAR(50), @mdlNme VARCHAR(25), @lstNme VARCHAR(25))
RETURNS VARCHAR(102) AS
BEGIN
	RETURN CONCAT(@name, ' ',@mdlNme, ' ',@lstNme);
END
GO



--Selective Selects
IF OBJECT_ID('userValidate') IS NOT NULL
	DROP PROC userValidate
GO
CREATE PROC userValidate @user VARCHAR(25), @pass VARCHAR(25) AS
	SELECT COORDINADOR.clave_Usuario,COORDINADOR.titulo FROM COORDINADOR WHERE COORDINADOR.clave_Usuario = @user AND COORDINADOR.contraseña = @pass
GO	

IF OBJECT_ID('careerAverages') IS NOT NULL
	DROP PROC careerAverages
GO
CREATE PROC careerAverages @semestral BIT = 0, @carrer INT = 0 AS
	
	DECLARE @table TABLE(Promedio FLOAT,Carrera VARCHAR(25), IdCareer INT);
	IF @semestral = 1
		INSERT INTO @table(Promedio,Carrera,IdCareer) SELECT	AVG(ALUMNO_GRUPO.calificacion),CARRERA.alias,CARRERA.clave
				FROM ALUMNO_GRUPO INNER JOIN GRUPO ON ALUMNO_GRUPO.grupo_fk = GRUPO.id 
				INNER JOIN MATERIA_CARRERA ON GRUPO.materia_fk = MATERIA_CARRERA.materia_fk
				INNER JOIN CARRERA ON MATERIA_CARRERA.carrera_fk = CARRERA.clave
				GROUP BY CARRERA.alias,GRUPO.semestre_fk,CARRERA.clave
	ELSE
		INSERT INTO @table(Promedio,Carrera,IdCareer) SELECT	AVG(ALUMNO_GRUPO.calificacion),CARRERA.alias,CARRERA.clave
				FROM ALUMNO_GRUPO INNER JOIN GRUPO ON ALUMNO_GRUPO.grupo_fk = GRUPO.id 
				INNER JOIN MATERIA_CARRERA ON GRUPO.materia_fk = MATERIA_CARRERA.materia_fk
				INNER JOIN CARRERA ON MATERIA_CARRERA.carrera_fk = CARRERA.clave
				GROUP BY CARRERA.alias,CARRERA.clave

	IF @carrer >= 500
		SELECT Carrera,CONVERT(decimal(5,2),Promedio) Promedio FROM @table WHERE IdCareer = @carrer
	ELSE
		SELECT Carrera,CONVERT(decimal(5,2),Promedio) Promedio FROM @table
GO

IF OBJECT_ID('subjectAverages') IS NOT NULL
	DROP PROC subjectAverages
GO
CREATE PROC subjectAverages @option int = 0, @subject INT = -1 AS
	
	DECLARE @table TABLE(Promedio FLOAT,tSubject VARCHAR(25), IdSubject INT);
	IF @option = 0
		INSERT INTO @table(Promedio,tSubject,IdSubject) SELECT	AVG(ALUMNO_GRUPO.calificacion),MATERIA.nombre,MATERIA.clave
				FROM ALUMNO_GRUPO INNER JOIN GRUPO ON ALUMNO_GRUPO.grupo_fk = GRUPO.id 
				INNER JOIN MATERIA ON GRUPO.materia_fk = MATERIA.clave
				GROUP BY MATERIA.clave,MATERIA.nombre
	ELSE IF @option = 1
		INSERT INTO @table(Promedio,tSubject,IdSubject) SELECT	AVG(ALUMNO_GRUPO.calificacion),MATERIA.nombre,MATERIA.clave
				FROM ALUMNO_GRUPO INNER JOIN GRUPO ON ALUMNO_GRUPO.grupo_fk = GRUPO.id 
				INNER JOIN MATERIA ON GRUPO.materia_fk = MATERIA.clave
				GROUP BY MATERIA.clave,MATERIA.nombre,GRUPO.semestre_fk
	ELSE
	INSERT INTO @table(Promedio,tSubject,IdSubject) SELECT	AVG(ALUMNO_GRUPO.calificacion),MATERIA.nombre,MATERIA.clave
				FROM ALUMNO_GRUPO INNER JOIN GRUPO ON ALUMNO_GRUPO.grupo_fk = GRUPO.id 
				INNER JOIN MATERIA ON GRUPO.materia_fk = MATERIA.clave
				GROUP BY MATERIA.clave,MATERIA.nombre,GRUPO.id

	IF @subject != -1
		SELECT tSubject Materia, CONVERT(decimal(5,2),Promedio) Promedio FROM @table WHERE IdSubject = @subject
	ELSE
		SELECT tSubject Materia, CONVERT(decimal(5,2),Promedio) Promedio FROM @table
GO

IF OBJECT_ID('semesterAverages') IS NOT NULL
	DROP PROC semesterAverages
GO
CREATE PROC semesterAverages @option int = 0, @semester INT = -1 AS
	
	DECLARE @table TABLE(Promedio FLOAT, IdSemester INT);
	IF @option = 0
		INSERT INTO @table(Promedio,IdSemester) SELECT	AVG(ALUMNO_GRUPO.calificacion),GRUPO.semestre_fk
				FROM ALUMNO_GRUPO INNER JOIN GRUPO ON ALUMNO_GRUPO.grupo_fk = GRUPO.id 
				GROUP BY GRUPO.semestre_fk,GRUPO.id
	ELSE IF @option = 1
		INSERT INTO @table(Promedio,IdSemester) SELECT	AVG(ALUMNO_GRUPO.calificacion),GRUPO.semestre_fk
				FROM ALUMNO_GRUPO INNER JOIN GRUPO ON ALUMNO_GRUPO.grupo_fk = GRUPO.id 
				INNER JOIN MATERIA ON GRUPO.materia_fk = MATERIA.clave
				GROUP BY MATERIA.clave,GRUPO.semestre_fk
	ELSE IF @option = 2
	INSERT INTO @table(Promedio,IdSemester) SELECT	AVG(ALUMNO_GRUPO.calificacion),GRUPO.semestre_fk
				FROM ALUMNO_GRUPO INNER JOIN GRUPO ON ALUMNO_GRUPO.grupo_fk = GRUPO.id 
				INNER JOIN MATERIA_CARRERA ON GRUPO.materia_fk = MATERIA_CARRERA.materia_fk
				GROUP BY MATERIA_CARRERA.carrera_fk,GRUPO.semestre_fk
	ELSE
		INSERT INTO @table(Promedio,IdSemester) SELECT	AVG(ALUMNO_GRUPO.calificacion),GRUPO.semestre_fk
					FROM ALUMNO_GRUPO INNER JOIN GRUPO ON ALUMNO_GRUPO.grupo_fk = GRUPO.id 
					GROUP BY GRUPO.semestre_fk

	IF @semester != -1
		SELECT dbo.semesterName(IdSemester) Semestre,CONVERT(decimal(5,2),Promedio) Promedio FROM @table WHERE IdSemester = @semester
	ELSE
		SELECT dbo.semesterName(IdSemester) Semestre,CONVERT(decimal(5,2),Promedio) Promedio FROM @table
GO

IF OBJECT_ID('subjectMultySelect') IS NOT NULL
	DROP PROC subjectMultySelect
GO
CREATE PROC subjectMultySelect	@subject INT = NULL, @subjectNick VARCHAR(25) = NULL, @subjectName VARCHAR(50) = NULL, @option INT AS
	
	IF (@subject IS NULL OR @subject < 100) AND @option != 5
	BEGIN
		SELECT @subject = MATERIA.clave FROM MATERIA WHERE (MATERIA.alias = @subjectNick AND @subjectNick IS NOT NULL) OR (MATERIA.nombre = @subjectName AND @subjectName IS NOT NULL);
		IF @subject IS NULL
		BEGIN
			RETURN -1;
		END
	END
	
	IF @option = 0	--subjectTeachers
	BEGIN
		SELECT	MAESTRO.matricula,CONCAT(MAESTRO.nombre,' ',MAESTRO.apellido_Pat,' ',MAESTRO.apellido_Mat) AS nombre_completo	FROM MAESTRO INNER JOIN MAESTRO_MATERIA 
				ON MAESTRO.matricula = MAESTRO_MATERIA.maestro_fk WHERE MAESTRO_MATERIA.materia_fk = @subject;
	END
	ELSE IF @option = 1	--subjectCredits
	BEGIN
		SELECT	MATERIA.creditos FROM MATERIA WHERE MATERIA.clave = @subject;
	END
	ELSE IF @option = 2	--subjectMandatoryHours
	BEGIN
		SELECT MATERIA.medias_horas_Sem FROM MATERIA WHERE MATERIA.clave = @subject; 
	END
	ELSE IF @option = 3 --oneSubjectAll
	BEGIN 
		SELECT	MATERIA.nombre AS Nombre,MATERIA.alias AS Alias,MATERIA.creditos AS Creditos,	
				MATERIA.medias_horas_Sem AS '½Hrs/Sem',MATERIA.nivel AS Nivel, MATERIA.descripcion as Descripcion FROM MATERIA
				WHERE MATERIA.clave = @subject;
	END
	ELSE IF @option = 4 --allSubjectsForCheck
	BEGIN
		SELECT	MATERIA.clave AS Clave,MATERIA.nombre AS Nombre FROM MATERIA;
	END
	ELSE IF @option = 5	--allSubjectsAll
	BEGIN
		SELECT	MATERIA.clave AS Clave,MATERIA.nombre AS Nombre,MATERIA.alias AS Alias,MATERIA.creditos AS Creditos,	
				MATERIA.medias_horas_Sem AS '½Hrs/Sem',MATERIA.nivel AS Nivel, MATERIA.descripcion as Descripcion FROM MATERIA;
	END
	ELSE IF @option = 6
	BEGIN
		DELETE FROM MATERIA WHERE MATERIA.clave = @subject;
	END
	ELSE IF @option = 7
	BEGIN
		SELECT	MATERIA.clave AS Clave,MATERIA.nombre AS Nombre,MATERIA.alias AS Alias,MATERIA.creditos AS Creditos, MATERIA.nivel AS Nivel 
				FROM MATERIA LEFT JOIN BLOQUEO_MATERIA ON MATERIA.clave = BLOQUEO_MATERIA.bloque_fk
				WHERE BLOQUEO_MATERIA.materia_fk = @subject;
	END
	ELSE IF @option = 8
	BEGIN
		DECLARE @lvl INT
		SELECT @lvl = MATERIA.nivel FROM MATERIA WHERE MATERIA.clave = @subject
		SELECT	MATERIA.nombre AS Nombre,MATERIA.alias AS Alias,MATERIA.clave AS Clave, MATERIA.nivel AS Nivel 
				FROM MATERIA WHERE MATERIA.clave NOT IN(SELECT BLOQUEO_MATERIA.materia_fk FROM BLOQUEO_MATERIA 
				WHERE BLOQUEO_MATERIA.bloque_fk = @subject) AND MATERIA.clave NOT IN(SELECT BLOQUEO_MATERIA.bloque_fk FROM BLOQUEO_MATERIA 
				WHERE BLOQUEO_MATERIA.materia_fk = @subject) AND MATERIA.clave != @subject AND MATERIA.nivel < @lvl;
	END
GO

IF OBJECT_ID('careerMultySelect') IS NOT NULL
	DROP PROC careerMultySelect
GO
CREATE PROC careerMultySelect  @career INT = NULL, @careerNick VARCHAR(25) = NULL, @careerName VARCHAR(50) = NULL,@option INT AS
	
	IF (@career IS NULL OR @career < 500) AND @option != 5
	BEGIN
		SELECT @career = MATERIA.clave FROM MATERIA WHERE (MATERIA.alias = @careerNick AND @careerNick IS NOT NULL) OR (MATERIA.nombre = @careerName AND @careerName IS NOT NULL);
		IF @career IS NULL
		BEGIN
			RETURN -1;
		END
	END
	
	IF @option = 0 --allCareersAll
	BEGIN 
		SELECT CARRERA.clave AS Clave,CARRERA.nombre AS Nombre,CARRERA.alias AS Alias,CARRERA.descripcion AS Descripcion FROM CARRERA
	END
	ELSE IF @option = 1 --careersWithoutCoordinator
	BEGIN
		SELECT CARRERA.nombre AS Nombre FROM CARRERA WHERE CARRERA.clave NOT IN 
			   (SELECT CARRERA.clave FROM CARRERA INNER JOIN COORDINADOR ON CARRERA.clave = COORDINADOR.carrera_fk)
	END
	ELSE IF @option = 2 --oneCareerAll
	BEGIN
		SELECT CARRERA.clave AS Clave,CARRERA.nombre AS Nombre,CARRERA.alias AS Alias,CARRERA.descripcion AS Descripcion FROM CARRERA
			WHERE CARRERA.clave = @career
	END
	ELSE IF @option = 3
	BEGIN
		IF NOT EXISTS(SELECT ALUMNO.matricula FROM ALUMNO WHERE ALUMNO.carrera_fk = @career)
		BEGIN
			DELETE FROM CARRERA WHERE CARRERA.clave = @career
		END
		ELSE
		BEGIN
			SELECT 'Hay alumnos inscritos a la carrera, no se puede eliminar' ERROR
		END
	END
	ELSE IF @option = 4
	BEGIN
		SELECT	MATERIA.clave AS Clave,MATERIA.nombre AS Nombre,MATERIA.alias AS Alias,MATERIA.creditos AS Creditos,MATERIA.nivel AS Nivel, MATERIA.descripcion as Descripcion 
				FROM MATERIA INNER JOIN MATERIA_CARRERA ON MATERIA.clave = MATERIA_CARRERA.materia_fk
				WHERE MATERIA_CARRERA.carrera_fk = @career
	END
	ELSE IF @option = 5
	BEGIN
		SELECT	MATERIA.clave AS Clave,MATERIA.nombre AS Nombre,MATERIA.alias AS Alias,MATERIA.creditos AS Creditos,MATERIA.nivel AS Nivel, MATERIA.descripcion as Descripcion 
				FROM MATERIA WHERE MATERIA.clave NOT IN(SELECT MATERIA_CARRERA.materia_fk FROM MATERIA_CARRERA
				WHERE MATERIA_CARRERA.carrera_fk = @career)
	END
GO

IF OBJECT_ID('studentMultySelect') IS NOT NULL
	DROP PROC studentMultySelect
GO
CREATE PROC studentMultySelect @student INT, @option INT AS
	IF @option = 0 --studentEvaluation
	BEGIN
		SELECT MATERIA.nombre Materia,ALUMNO_GRUPO.calificacion Calificacion 
		FROM GRUPO INNER JOIN MATERIA ON GRUPO.materia_fk = MATERIA.clave INNER JOIN ALUMNO_GRUPO
				ON ALUMNO_GRUPO.grupo_fk = GRUPO.id 
				INNER JOIN dbo.thisSemester ON GRUPO.semestre_fk = dbo.thisSemester.id
				WHERE ALUMNO_GRUPO.alumno_fk = @student	
				ORDER BY MATERIA.nivel ASC, MATERIA.nombre DESC;
	END
	ELSE IF @option = 1 --studentKardex
	BEGIN
		SELECT	MATERIA.nombre Materia,MAX(ALUMNO_GRUPO.calificacion) Calificacion FROM GRUPO INNER JOIN MATERIA ON GRUPO.materia_fk = MATERIA.clave INNER JOIN ALUMNO_GRUPO
				ON ALUMNO_GRUPO.grupo_fk = GRUPO.id WHERE ALUMNO_GRUPO.alumno_fk = @student
				GROUP BY MATERIA.nombre,MATERIA.nivel
				ORDER BY MATERIA.nivel ASC, MATERIA.nombre DESC;
	END
	ELSE IF @option = 2 --allStudentsSome
	BEGIN
		SELECT	studentsInfo.Matricula,studentsInfo.Nombre, 
				studentsInfo.Carrera, studentsInfo.Estado 
				FROM studentsInfo
	END
	ELSE IF @option = 3 --oneStudentAll
	BEGIN
		SELECT	studentsInfo.matricula,studentsInfo.nombre,studentsInfo.apellido_paterno,studentsInfo.apellido_materno,studentsInfo.genero,studentsInfo.Nacimiento,studentsInfo.carrera,studentsInfo.pais
				FROM studentsInfo WHERE studentsInfo.matricula = @student;
	END
	ELSE IF @option = 4 --deleteStudent
	BEGIN
		IF NOT EXISTS(SELECT SEMESTRE.id FROM SEMESTRE WHERE SEMESTRE.estado = 1)
		BEGIN
			UPDATE ALUMNO SET activo = 0 WHERE ALUMNO.matricula = @student;
		END
	END
GO

IF OBJECT_ID('coordinatorMultySelect') IS NOT NULL
	DROP PROC coordinatorMultySelect
GO
CREATE PROC coordinatorMultySelect @coordinator VARCHAR(50) = '', @option INT AS
	IF @option = 0 --allCoordinatorsSome
	BEGIN
		SELECT	COORDINADOR.clave_Usuario Usuario,CONCAT(COORDINADOR.nombre,' ',COORDINADOR.apellido_Pat,' ',COORDINADOR.apellido_Mat) 'Nombre Completo', 
				CARRERA.nombre 'Carrera que Coordina', COORDINADOR.titulo Titulo
				FROM COORDINADOR INNER JOIN CARRERA ON COORDINADOR.carrera_fk = CARRERA.clave 
	END
	ELSE IF @option = 1 --oneCoordinatorAll
	BEGIN
		SELECT	COORDINADOR.clave_Usuario,COORDINADOR.contraseña,COORDINADOR.nombre,COORDINADOR.apellido_Pat,COORDINADOR.apellido_Mat,COORDINADOR.genero,
				COORDINADOR.fecha_Naci,CARRERA.nombre carrera,COORDINADOR.num_nomina,COORDINADOR.titulo
				FROM COORDINADOR INNER JOIN CARRERA ON COORDINADOR.carrera_fk = CARRERA.clave WHERE COORDINADOR.clave_Usuario = @coordinator;
	END
	ELSE IF @option = 2 --deleteCoordinator
	BEGIN
		DELETE FROM COORDINADOR WHERE COORDINADOR.clave_Usuario = @coordinator
	END
GO

IF OBJECT_ID('teacherMultySelect') IS NOT NULL
	DROP PROC teacherMultySelect
GO
CREATE PROC teacherMultySelect @teacher INT = 0, @option INT AS
	IF @option = 0 --allTeachersSome
	BEGIN
		SELECT	MAESTRO.matricula Matricula, CONCAT(MAESTRO.nombre,' ',MAESTRO.apellido_Pat,' ',MAESTRO.apellido_Mat) 'Nombre Completo',
				MAESTRO.titulo Titulo FROM MAESTRO
	END
	ELSE IF @option = 1 --oneTeacherAll
	BEGIN
		SELECT	MAESTRO.nombre,MAESTRO.apellido_Pat,MAESTRO.apellido_Mat,MAESTRO.fecha_Naci,
				MAESTRO.genero,MAESTRO.num_nomina,MAESTRO.titulo FROM MAESTRO WHERE MAESTRO.matricula = @teacher
	END
	ELSE IF @option = 2 --deleteTeacher
	BEGIN
		IF NOT EXISTS(SELECT GRUPO.id FROM GRUPO INNER JOIN SEMESTRE ON GRUPO.semestre_fk = SEMESTRE.id WHERE GRUPO.maestro_fk = @teacher AND SEMESTRE.estado = 1)
		BEGIN
			DELETE FROM MAESTRO WHERE MAESTRO.matricula = @teacher
		END
		ELSE
		BEGIN
			SELECT 'No se puede eliminar el maestro porque existen grupos en los cuales está enseñando' ERROR
		END
	END
	ELSE IF @option = 3 --teacherSubjects
	BEGIN
		SELECT	MATERIA.nombre AS Nombre,MATERIA.alias AS Alias,MATERIA.clave AS Clave, MATERIA.nivel AS Nivel
				FROM MAESTRO_MATERIA INNER JOIN MATERIA ON MAESTRO_MATERIA.materia_fk = MATERIA.clave
				WHERE MAESTRO_MATERIA.maestro_fk = @teacher
	END
	ELSE IF @option = 4 --teacherOcupiedHours
	BEGIN
		SELECT hora,lunes,martes,miercoles,jueves,viernes,sabado FROM groupHrsJoin
		   WHERE groupHrsJoin.maestro_fk = @teacher;
	END
	ELSE IF @option = 5 --notTeacherSubjects
	BEGIN
		SELECT	MATERIA.nombre AS Nombre,MATERIA.alias AS Alias,MATERIA.clave AS Clave, MATERIA.nivel AS Nivel
				FROM MATERIA WHERE MATERIA.clave NOT IN(SELECT	MATERIA.clave
				FROM MAESTRO_MATERIA INNER JOIN MATERIA ON MAESTRO_MATERIA.materia_fk = MATERIA.clave
				WHERE MAESTRO_MATERIA.maestro_fk = @teacher)
	END
GO

IF OBJECT_ID('classroomMultySelect') IS NOT NULL
	DROP PROC classroomMultySelect
GO
CREATE PROC classroomMultySelect @classroom INT = -1,@option INT AS
	IF @option = 0 --classroomOcupiedHours
	BEGIN
		SELECT hora,lunes,martes,miercoles,jueves,viernes,sabado FROM groupHrsJoin
		   WHERE groupHrsJoin.aula_fk = @classroom
	END
	ELSE IF @option = 1 --allClassroomAll
	BEGIN
		SELECT AULA.id,AULA.localizacion,AULA.cupo FROM AULA
	END
	ELSE IF @option = 2 --oneClassroomAll
	BEGIN
		SELECT AULA.id,AULA.localizacion,AULA.cupo FROM AULA WHERE AULA.id = @classroom
	END
GO

IF OBJECT_ID('groupMultySelect') IS NOT NULL
	DROP PROC groupMultySelect
GO
CREATE PROC groupMultySelect @group INT = -1,@option INT AS
	IF @option = 0 --groupOcupiedHours
	BEGIN
		SELECT hora,lunes,martes,miercoles,jueves,viernes,sabado FROM groupHrsJoin
		   WHERE groupHrsJoin.grupo_fk = @group
	END
	ELSE IF @option = 1 --allGroupsAll
	BEGIN
		SELECT	GRUPO.id,MATERIA.nombre Materia,CONCAT(MAESTRO.nombre,' ',MAESTRO.apellido_Pat,' ',MAESTRO.apellido_Mat) 'Maestro',
				AULA.localizacion Aula FROM GRUPO INNER JOIN MAESTRO ON GRUPO.maestro_fk = MAESTRO.matricula
				INNER JOIN MATERIA ON GRUPO.materia_fk = MATERIA.clave 
				INNER JOIN AULA ON GRUPO.aula_fk = AULA.id
				INNER JOIN thisSemester ON GRUPO.semestre_fk = thisSemester.id
	END
	ELSE IF @option = 2 --oneGroupAll
	BEGIN
		SELECT	GRUPO.id,MATERIA.nombre Materia,CONCAT(MAESTRO.nombre,' ',MAESTRO.apellido_Pat,' ',MAESTRO.apellido_Mat) Maestro,
				AULA.localizacion Aula FROM GRUPO INNER JOIN MAESTRO ON GRUPO.maestro_fk = MAESTRO.matricula
				INNER JOIN MATERIA ON GRUPO.materia_fk = MATERIA.clave 
				INNER JOIN AULA ON GRUPO.aula_fk = AULA.id
				WHERE GRUPO.id = @group
	END
	ELSE IF @option = 3 -- deleteGroup
	BEGIN
		IF NOT EXISTS(SELECT ALUMNO_GRUPO.id FROM ALUMNO_GRUPO INNER JOIN ALUMNO ON ALUMNO_GRUPO.alumno_fk = ALUMNO.matricula WHERE ALUMNO_GRUPO.grupo_fk = @group AND ALUMNO.activo = 1)
		BEGIN
			DELETE FROM HORARIO_GRUPO WHERE HORARIO_GRUPO.grupo_fk = @group
			DELETE FROM ALUMNO_GRUPO WHERE ALUMNO_GRUPO.grupo_fk = @group
			DELETE FROM GRUPO WHERE GRUPO.id = @group
		END
		ELSE
		BEGIN
			SELECT 'Hay alumnos inscritos a este grupo, no se puede eliminar' ERROR
		END
	END
	ELSE IF @option = 4 --groupStudents
	BEGIN
		SELECT	 studentsInfo.Matricula,studentsInfo.Nombre_Completo Nombre,ALUMNO_GRUPO.calificacion Calificacion,studentsInfo.Carrera
				 FROM ALUMNO_GRUPO INNER JOIN studentsInfo ON ALUMNO_GRUPO.alumno_fk = studentsInfo.Matricula
				 WHERE ALUMNO_GRUPO.grupo_fk = @group AND studentsInfo.Estado = 'Activo'
				 ORDER BY studentsInfo.Nombre_Completo
	END
	ELSE IF @option = 5 --groupNotStudents
	BEGIN
		DECLARE @subject INT;
		DECLARE @semester INT;
		SELECT @semester = GRUPO.semestre_fk, @subject =GRUPO.materia_fk FROM GRUPO WHERE GRUPO.id = @group

		SELECT	ALUMNO.matricula Matricula, dbo.f_fullName(ALUMNO.nombre,ALUMNO.apellido_Pat,ALUMNO.apellido_Mat) Nombre
		FROM ALUMNO INNER JOIN groupsPerCareer ON ALUMNO.carrera_fk = groupsPerCareer.Carrera
		WHERE ALUMNO.activo = 1 AND groupsPerCareer.Grupo = @group AND ALUMNO.matricula NOT IN(
		SELECT ALUMNO_GRUPO.alumno_fk FROM ALUMNO_GRUPO INNER JOIN GRUPO ON ALUMNO_GRUPO.grupo_fk = GRUPO.id
		WHERE (GRUPO.id != @group AND GRUPO.materia_fk = @subject OR GRUPO.id = @group) AND GRUPO.semestre_fk = @semester)
		GROUP BY ALUMNO.matricula,dbo.f_fullName(ALUMNO.nombre,ALUMNO.apellido_Pat,ALUMNO.apellido_Mat)
		HAVING AVG(dbo.isStudentAproved(ALUMNO.matricula,groupsPerCareer.Bloqueo)) = 1
	END
GO

IF OBJECT_ID('semesterMultySelect') IS NOT NULL
	DROP PROC semesterMultySelect
GO
CREATE PROC semesterMultySelect @option INT, @semester INT = -1 AS
	IF @option = 0 --allSemesters
	BEGIN
		SELECT SEMESTRE.id,dbo.semesterName(SEMESTRE.id) Semestre,CASE WHEN SEMESTRE.estado = 1 THEN 'Actual' ELSE CASE WHEN DATEDIFF(DAY,GETDATE(),SEMESTRE.fecha_Inicio) > 0 THEN 'Por Venir' ELSE 'Finalizado' END END AS Estado FROM SEMESTRE
	END
	ELSE IF @option = 1 --beginNextSemester
	BEGIN
		IF NOT EXISTS(SELECT SEMESTRE.estado FROM SEMESTRE WHERE SEMESTRE.estado = 1)
		BEGIN
			UPDATE dbo.nextSemester SET estado = 1,fecha_Inicio = GETDATE()
			IF NOT EXISTS(SELECT SEMESTRE.estado FROM SEMESTRE WHERE SEMESTRE.estado = 1)
				SELECT 'No hay un semestre proximo en la base de datos' ERROR
			ELSE
				SELECT dbo.semesterName(SEMESTRE.id) Semestre,SEMESTRE.id FROM SEMESTRE WHERE SEMESTRE.estado = 1
		END
		ELSE
			SELECT 'Hay otro semestre activo' ERROR
	END
	ELSE IF @option = 2 --endLastSemester
	BEGIN
		IF NOT EXISTS(SELECT ALUMNO_GRUPO.id FROM ALUMNO_GRUPO INNER JOIN ALUMNO ON ALUMNO.matricula = ALUMNO_GRUPO.alumno_fk WHERE ALUMNO_GRUPO.calificacion IS NULL AND ALUMNO.activo = 1)
		BEGIN
			DECLARE @table TABLE(ID INT)
			UPDATE SEMESTRE SET fecha_fin = GETDATE(),estado = 0 OUTPUT deleted.id INTO @table WHERE SEMESTRE.estado = 1 
			SELECT dbo.semesterName(ID) Semestre, ID FROM @table
		END
		ELSE
			SELECT 'Aún hay alumnos sin calificación' ERROR
	END
	ELSE IF @option = 3 --thisSemester
	BEGIN
	 SELECT dbo.semesterName(SEMESTRE.id) Semestre,SEMESTRE.id FROM SEMESTRE WHERE estado = 1
	END
	ELSE IF @option = 4 --oneSemesterAll
	BEGIN
		SELECT	SEMESTRE.creditos_Min,SEMESTRE.creditos_Max,SEMESTRE.fecha_Inicio,SEMESTRE.fecha_fin
				FROM SEMESTRE WHERE SEMESTRE.id = @semester
	END
	ELSE IF @option = 5
	BEGIN
		IF NOT EXISTS(SELECT SEMESTRE.id FROM SEMESTRE WHERE SEMESTRE.estado = 1 AND SEMESTRE.id = @semester)
			DELETE FROM SEMESTRE WHERE SEMESTRE.id = @semester
		ELSE
			SELECT 'Está intentando eliminar el semestre activo actualmente' ERROR
	END
GO

IF OBJECT_ID('allCountryNames') IS NOT NULL
	DROP PROC allCountryNames
GO
CREATE PROC allCountryNames AS
	SELECT PAIS.nombre FROM PAIS
GO

--Inserts
IF OBJECT_ID('insertStudent') IS NOT NULL
	DROP PROC insertStudent
GO
CREATE PROC insertStudent
			@ID			INT = NULL,
			@name		VARCHAR(50),
			@mdlnme		VARCHAR(50),
			@lstnme		VARCHAR(50),
			@gender		BIT = NULL,
			@birthdate	DATE = NULL,
			@career		VARCHAR(50),
			@country	VARCHAR(50)
			AS
	DECLARE @careerID INT;
	DECLARE @countryID INT;
	SELECT	@careerID=CARRERA.clave FROM CARRERA WHERE CARRERA.nombre = @career;
	SELECT	@countryID=PAIS.id FROM PAIS WHERE PAIS.nombre = @country;
	IF @ID < 1000000
	BEGIN
		INSERT INTO ALUMNO (nombre,apellido_Pat,apellido_Mat,genero,fecha_Naci,carrera_fk,pais_fk)
					VALUES (@name,@mdlnme,@lstnme,isnull(@gender,1),isnull(@birthdate,getdate()),@careerID,@countryID)
		RETURN 1;
	END
	ELSE
	BEGIN
		UPDATE	ALUMNO SET	nombre = @name,apellido_Pat = @mdlnme,apellido_Mat = @lstnme,genero = ISNULL(@gender, genero),
							fecha_Naci = ISNULL(@birthdate, fecha_Naci),carrera_fk = @careerID,pais_fk = @countryID
				WHERE ALUMNO.matricula = @ID
		RETURN 2;
	END
	RETURN 0;
GO

IF OBJECT_ID('insertClassroom') IS NOT NULL
	DROP PROC insertClassroom
GO
CREATE PROC insertClassroom @seats INT, @localization VARCHAR(255) = NULL AS
	INSERT INTO AULA(cupo,localizacion) VALUES(@seats,isnull(@localization,'No especificado'));
GO

IF OBJECT_ID('insertSubject') IS NOT NULL
	DROP PROC insertSubject
GO
CREATE PROC insertSubject 
			@code	INT = NULL,
			@name	VARCHAR(50),
			@nick	VARCHAR(25)  = NULL,
			@description VARCHAR(255) = NULL,
			@credits SMALLINT = 1,
			@halveHrs SMALLINT = 1,
			@level TINYINT = 1

AS
	IF @code >= 100
	BEGIN
		IF NOT EXISTS(SELECT SEMESTRE.estado FROM SEMESTRE WHERE SEMESTRE.estado = 1)
		BEGIN
		UPDATE dbo.MATERIA SET nombre = @name,alias = isnull(@nick,alias),descripcion = isnull(@description,descripcion),
					creditos = @credits,medias_horas_Sem = @halveHrs,nivel = @level
					WHERE MATERIA.clave = @code
		RETURN 1;
		END
		ELSE
		BEGIN
			RETURN -1;
		END
	END
	ELSE IF NOT EXISTS(SELECT MATERIA.nombre FROM MATERIA WHERE (MATERIA.nombre = @name OR (MATERIA.alias = @nick)))
	BEGIN
		INSERT INTO dbo.MATERIA(nombre,alias,descripcion,creditos,medias_horas_Sem,nivel) 
					VALUES(@name,isnull(@nick,'No especificado'),isnull(@description,'No especificado'),@credits,@halveHrs,@level)
		RETURN 1;
	END
	RETURN 0;
GO

IF OBJECT_ID('insertCareer') IS NOT NULL
	DROP PROC insertCareer
GO
CREATE PROC insertCareer @code	INT = NULL,@name VARCHAR(50), @nick VARCHAR(25) = NULL, @description VARCHAR(255) = NULL AS
	IF NOT EXISTS(SELECT CARRERA.nombre FROM CARRERA WHERE CARRERA.nombre = @name OR Carrera.alias = @nick)
	BEGIN
		IF @code >= 500
		BEGIN
			IF NOT EXISTS(SELECT SEMESTRE.estado FROM SEMESTRE WHERE SEMESTRE.estado = 1)
			BEGIN
				UPDATE CARRERA SET nombre = @name,alias = ISNULL(@nick,alias),descripcion = ISNULL(@description,descripcion)
				WHERE CARRERA.clave = @code
				return @code
			END
			ELSE
			BEGIN
				return -2;
			END
		END
		ELSE 
		BEGIN
			DECLARE @table table(clave int);
			INSERT INTO CARRERA(nombre,alias,descripcion) OUTPUT INSERTED.clave INTO @table
				   VALUES(@name,ISNULL(@nick,'No especificado'),ISNULL(@description,'No especificada')) 
			DECLARE @return INT;
			SELECT @return = clave FROM @table;
			RETURN @return;
		END
	END
	RETURN -1;
GO

IF OBJECT_ID('insertCoordinator') IS NOT NULL
	DROP PROC insertCoordinator
GO
CREATE PROC insertCoordinator 
			@name		VARCHAR(50),
			@sndnme		VARCHAR(50),
			@lstnme		VARCHAR(50),
			@gender		BIT = NULL,
			@birthdate	DATE = NULL,
			@career		VARCHAR(50),
			@degree		VARCHAR(50) = NULL,
			@number		VARCHAR(50),
			@user		VARCHAR(50),
			@password	VARCHAR(50),
			@update BIT = 0
			AS
	DECLARE @CAREERID INT;
	SELECT @CAREERID = CARRERA.clave FROM CARRERA WHERE CARRERA.nombre = @career;
	IF @update = 0
	BEGIN
		IF NOT EXISTS(SELECT COORDINADOR.nombre FROM COORDINADOR WHERE COORDINADOR.clave_Usuario = @user)
		BEGIN
			INSERT INTO COORDINADOR(nombre,apellido_Pat,apellido_Mat,genero,fecha_Naci,carrera_fk,titulo,num_nomina,clave_Usuario,contraseña)
						VALUES(@name,@sndnme,@lstnme,isnull(@gender,1),isnull(@birthdate,getdate()),@CAREERID,isnull(@degree,'Profesor/a'),@number,@user,@password)
			RETURN 1
		END
		RETURN 0
	END
	ELSE
	BEGIN
		UPDATE COORDINADOR SET	nombre = @name,apellido_Pat = @sndnme,apellido_Mat = @lstnme,genero = @gender,fecha_Naci = @birthdate,carrera_fk = @CAREERID,titulo = @degree,num_nomina = @number
			   WHERE COORDINADOR.clave_Usuario = @user AND COORDINADOR.contraseña = @password
		RETURN 2
	END
	RETURN -1
GO

IF OBJECT_ID('insertGroup') IS NOT NULL
	DROP PROC insertGroup
GO
CREATE PROC insertGroup @teacher INT, @subject INT, @classroom INT, @group INT = -1 AS
	DECLARE @SEME INT
	IF @group = -1
	BEGIN
		DECLARE @table table(clave int);
		SELECT @SEME=id FROM thisSemester
		INSERT	INTO GRUPO(maestro_fk,materia_fk,aula_fk,semestre_fk) OUTPUT INSERTED.id INTO @table
				VALUES(@teacher,@subject,@classroom,@SEME)
		DECLARE @return INT;
		SELECT @return = clave FROM @table;
		RETURN @return;
	END
	ELSE 
	BEGIN
		SELECT @SEME=id FROM thisSemester
		UPDATE	GRUPO SET maestro_fk = @teacher,materia_fk = @subject,aula_fk = @classroom,semestre_fk = @SEME
				WHERE GRUPO.id = @group;
		RETURN @group;
	END
GO

IF OBJECT_ID('insertTeacher') IS NOT NULL
	DROP PROC insertTeacher
GO
CREATE PROC insertTeacher
			@id			INT = 0,	
			@name		VARCHAR(50),
			@sndnme		VARCHAR(50),
			@lstnme		VARCHAR(50),
			@gender		BIT = NULL,
			@birthdate	DATE = NULL,
			@degree		VARCHAR(50) = NULL,
			@number		VARCHAR(50)
			AS
	IF @id = 0
	BEGIN
		DECLARE @table table(clave int);
		INSERT INTO MAESTRO(nombre,apellido_Pat,apellido_Mat,genero,fecha_Naci,titulo,num_nomina) OUTPUT INSERTED.matricula INTO @table
				VALUES(@name,@sndnme,@lstnme,isnull(@gender,1),isnull(@birthdate,getdate()),isnull(@degree,'Profesor/a'),@number)
		DECLARE @return INT;
		SELECT @return = clave FROM @table;
		RETURN @return;
	END
	ELSE IF @id > 0
	BEGIN
		UPDATE	MAESTRO SET nombre = @name,apellido_Pat = @sndnme,apellido_Mat = @lstnme,genero = isnull(@gender,genero),fecha_Naci = isnull(@birthdate,fecha_Naci),titulo = isnull(@degree,titulo),num_nomina = @number
				WHERE MAESTRO.matricula = @id
		RETURN @id;
	END
	RETURN -1
GO

IF OBJECT_ID('insertSemester') IS NOT NULL
	DROP PROC insertSemester
GO
CREATE PROC insertSemester @min SMALLINT, @max SMALLINT, @start DATE, @end DATE, @semester INT = -1 AS
	IF @semester = -1
	BEGIN
		INSERT	INTO SEMESTRE(creditos_Min,creditos_Max,fecha_Inicio,fecha_fin,estado) VALUES (@min,@max,@start,@end,0)
		RETURN 1
	END
	ELSE 
	BEGIN
		UPDATE	SEMESTRE SET creditos_Min = @min,creditos_Max = @max,fecha_Inicio = @start,fecha_fin = @end WHERE SEMESTRE.id = @semester;
		RETURN 1
	END
	RETURN 0
GO

		--inserts de tablas relacionales
				--usadas justo después de hacer el insert base
IF OBJECT_ID('groupHours') IS NOT NULL
	DROP PROC groupHours
GO
CREATE PROC	groupHours
			@group	INT,
			@hour	TIME,
			@monday BIT = 0,
			@tuesday BIT = 0,
			@wednesday BIT = 0,
			@thursday BIT = 0,
			@friday BIT = 0,
			@saturday BIT = 0,
			@update BIT = 0
AS
	IF @update = 0
	BEGIN
		DECLARE @ID INT = NULL;
		SELECT @ID = HORARIO.id FROM HORARIO 
				WHERE hora = @hour AND lunes = @monday AND martes = @tuesday AND miercoles = @wednesday 
			    AND jueves = @thursday AND viernes = @friday AND sabado = @saturday
		IF @ID IS NULL
		BEGIN
			DECLARE @table table(clave int);
			INSERT INTO HORARIO(hora,lunes,martes,miercoles,jueves,viernes,sabado) OUTPUT INSERTED.id INTO @table
						VALUES(@hour,@monday,@tuesday,@wednesday,@thursday,@friday,@saturday)
			SELECT @ID = clave FROM @table;
		END
		INSERT INTO HORARIO_GRUPO(grupo_fk,horario_fk) VALUES (@group,@ID)
	END
	ELSE
	BEGIN
		DELETE FROM HORARIO_GRUPO WHERE HORARIO_GRUPO.grupo_fk = @group
	END
GO

IF OBJECT_ID('teacherSubject') IS NOT NULL
	DROP PROC teacherSubject
GO
CREATE PROC teacherSubject @teacher INT, @subject INT, @remove BIT AS
	IF @remove = 0
	BEGIN
		INSERT INTO MAESTRO_MATERIA (maestro_fk,materia_fk) VALUES (@teacher,@subject)
		RETURN 1;
	END
	ELSE
	BEGIN
		IF NOT EXISTS(SELECT GRUPO.id FROM GRUPO INNER JOIN SEMESTRE ON GRUPO.semestre_fk = SEMESTRE.id WHERE GRUPO.maestro_fk = @teacher AND GRUPO.materia_fk = @subject AND SEMESTRE.estado = 1)
		BEGIN
			DELETE FROM MAESTRO_MATERIA WHERE MAESTRO_MATERIA.maestro_fk = @teacher AND MAESTRO_MATERIA.materia_fk = @subject
			RETURN 2;
		END
		ELSE 
		BEGIN
			RETURN -1;
		END
	END
GO

IF OBJECT_ID('subjectBlockSubject') IS NOT NULL
	DROP PROC subjectBlockSubject
GO
CREATE PROC subjectBlockSubject @subject INT, @block INT, @remove BIT AS
	IF @remove = 0
	BEGIN
		DECLARE @subjLvl TINYINT = 0;
		DECLARE @blockLvl TINYINT = 0;
		SELECT @subjLvl = MATERIA.nivel FROM MATERIA WHERE MATERIA.clave = @subject;
		SELECT @blockLvl = MATERIA.nivel FROM MATERIA WHERE MATERIA.clave = @block;
		IF @subject <> @block AND EXISTS(SELECT MATERIA.alias FROM MATERIA WHERE MATERIA.clave IN(@block,@subject)) AND @blockLvl < @subjLvl
		BEGIN
			INSERT INTO BLOQUEO_MATERIA(materia_fk,bloque_fk) VALUES(@subject,@block);
			RETURN 1;
		END
	END
	ELSE
	BEGIN
		DELETE FROM BLOQUEO_MATERIA WHERE materia_fk = @subject AND bloque_fk = @block;
		RETURN 2;
	END
	RETURN 0;
GO

IF OBJECT_ID('careerSubjects') IS NOT NULL
	DROP PROC careerSubjects
GO
CREATE PROC careerSubjects @career INT, @subject INT, @remove BIT AS
	IF @remove = 0
	BEGIN
		IF EXISTS(SELECT MATERIA.alias FROM MATERIA WHERE MATERIA.clave = @subject) AND 
		   EXISTS(SELECT CARRERA.alias FROM CARRERA WHERE CARRERA.clave = @career)
		BEGIN
			IF NOT EXISTS(SELECT MATERIA_CARRERA.id FROM MATERIA_CARRERA WHERE MATERIA_CARRERA.carrera_fk = @career AND MATERIA_CARRERA.materia_fk = @subject)
			BEGIN
				INSERT INTO MATERIA_CARRERA(carrera_fk,materia_fk) VALUES(@career,@subject)
			END
			RETURN 1;
		END
	END
	ELSE
	BEGIN
		DELETE FROM MATERIA_CARRERA WHERE carrera_fk = @career AND materia_fk = @subject
		RETURN 1;
	END
	RETURN 0;
GO

				--Usadas después para añadir informacion
IF OBJECT_ID('enrollStudentToGroup') IS NOT NULL
	DROP PROC enrollStudentToGroup
GO
CREATE PROC enrollStudentToGroup @student INT, @group INT, @remove BIT = 0 AS
	IF @remove = 0
	BEGIN
		IF EXISTS(SELECT ALUMNO.activo FROM ALUMNO WHERE ALUMNO.matricula = @student)	AND
		   EXISTS(SELECT GRUPO.id FROM GRUPO WHERE GRUPO.id = @group)
		BEGIN
			DECLARE @groupCount INT;
			DECLARE @groupMax INT;
			DECLARE @creditStudent INT;
			DECLARE @creditMax INT;
			SELECT	@groupCount = COUNT(ALUMNO_GRUPO.alumno_fk) FROM ALUMNO_GRUPO WHERE ALUMNO_GRUPO.grupo_fk = @group
					GROUP BY ALUMNO_GRUPO.grupo_fk

			SELECT	@groupMax = AULA.cupo,@creditStudent = MATERIA.creditos FROM AULA INNER JOIN GRUPO ON AULA.id = GRUPO.aula_fk
					INNER JOIN MATERIA ON GRUPO.materia_fk = MATERIA.clave
					WHERE GRUPO.id = @group

			SELECT	@creditStudent += SUM(MATERIA.creditos) FROM MATERIA INNER JOIN GRUPO ON MATERIA.clave = GRUPO.materia_fk
					INNER JOIN ALUMNO_GRUPO ON GRUPO.id = ALUMNO_GRUPO.grupo_fk 
					WHERE ALUMNO_GRUPO.alumno_fk = @student
					GROUP BY ALUMNO_GRUPO.alumno_fk

			SELECT	@creditMax = SEMESTRE.creditos_Max FROM SEMESTRE WHERE SEMESTRE.estado = 1 

			IF isnull(@groupCount,0) < @groupMax AND isnull(@creditStudent,0) <= @creditMax
			BEGIN
				INSERT INTO ALUMNO_GRUPO(alumno_fk,grupo_fk) VALUES (@student,@group)
			END
			ELSE
			BEGIN
				IF @groupCount >= @groupMax
					RETURN -1
				ELSE
					RETURN -2
			END
		END
	END
	ELSE
	BEGIN
		DELETE FROM ALUMNO_GRUPO WHERE ALUMNO_GRUPO.grupo_fk = @group AND ALUMNO_GRUPO.alumno_fk = @student
	END
GO

--update tables
IF OBJECT_ID('updateCalification') IS NOT NULL
	DROP PROC updateCalification
GO
CREATE PROC updateCalification @student INT, @group INT, @calification FLOAT AS
	UPDATE ALUMNO_GRUPO SET calificacion = @calification,fecha_calif = GETDATE() WHERE ALUMNO_GRUPO.alumno_fk = @student AND ALUMNO_GRUPO.grupo_fk = @group
GO

--triggers
IF OBJECT_ID('tg_blockSubjectDelete') IS NOT NULL
	DROP TRIGGER tg_blockSubjectDelete
GO
CREATE TRIGGER tg_blockSubjectDelete ON MATERIA AFTER DELETE AS
BEGIN
	DELETE FROM BLOQUEO_MATERIA WHERE bloque_fk IN(SELECT deleted.clave FROM deleted) OR materia_fk IN(SELECT deleted.clave FROM deleted)
END
GO

	--backup
IF OBJECT_ID('tg_bkp_PAIS') IS NOT NULL
	DROP TRIGGER tg_bkp_PAIS
GO
CREATE TRIGGER tg_bkp_PAIS ON PAIS AFTER DELETE,UPDATE AS
BEGIN
	DECLARE @ID INT;
	DECLARE @NAME VARCHAR(50);
	SELECT @ID=id,@NAME=nombre FROM deleted
	INSERT INTO PAIS_BKP(id,nombre) VALUES(@ID,@NAME) 
END
GO

IF OBJECT_ID('tg_bkp_CARRERA') IS NOT NULL
	DROP TRIGGER tg_bkp_CARRERA
GO
CREATE TRIGGER tg_bkp_CARRERA ON CARRERA AFTER DELETE,UPDATE AS
BEGIN
	DECLARE @ID INT;
	DECLARE @NAME VARCHAR(50);
	DECLARE @NICK VARCHAR(25);
	DECLARE @DESC VARCHAR(255);
	SELECT @ID=clave,@NAME=nombre,@NICK=alias,@DESC=descripcion FROM deleted
	INSERT INTO CARRERA_BKP(clave,nombre,alias,descripcion) VALUES(@ID,@NAME,@NICK,@DESC) 
END
GO

IF OBJECT_ID('tg_bkp_COORDINADOR') IS NOT NULL
	DROP TRIGGER tg_bkp_COORDINADOR
GO
CREATE TRIGGER tg_bkp_COORDINADOR ON COORDINADOR AFTER DELETE,UPDATE AS
BEGIN
	DECLARE @USER VARCHAR(50);
	DECLARE @PASS VARCHAR(50);
	DECLARE @NUMB VARCHAR(25);
	DECLARE @NAME VARCHAR(25);
	DECLARE @SNDN VARCHAR(25);
	DECLARE @LSTN VARCHAR(25);
	DECLARE @TITLE VARCHAR(50);
	DECLARE @GEND	BIT;
	DECLARE @FNAC	DATE;
	DECLARE @CARR INT;
	SELECT @USER=clave_Usuario,@PASS=contraseña,@NUMB=num_nomina,@NAME=nombre,@SNDN=apellido_Pat,@LSTN=apellido_Mat,@TITLE=titulo,@GEND=genero,@FNAC=fecha_Naci,@CARR=carrera_fk FROM deleted
	INSERT INTO COORDINADOR_BKP(clave_Usuario,contraseña,num_nomina,nombre,apellido_Pat,apellido_Mat,titulo,genero,fecha_Naci,carrera_fk) VALUES(@USER,@PASS,@NUMB,@NAME,@SNDN,@LSTN,@TITLE,@GEND,@FNAC,@CARR) 
END
GO

IF OBJECT_ID('tg_bkp_MATERIA') IS NOT NULL
	DROP TRIGGER tg_bkp_MATERIA
GO
CREATE TRIGGER tg_bkp_MATERIA ON MATERIA AFTER DELETE,UPDATE AS
BEGIN
	DECLARE @ID INT;
	DECLARE @NAME VARCHAR(50);
	DECLARE @NICK VARCHAR(25);
	DECLARE @DESC VARCHAR(255);
	DECLARE @CRED SMALLINT;
	DECLARE @HRS SMALLINT;
	DECLARE @LVL TINYINT;
	SELECT @ID=clave,@NAME=nombre,@NICK=alias,@DESC=descripcion,@CRED=creditos,@HRS=medias_horas_Sem,@LVL=nivel FROM deleted
	INSERT INTO MATERIA_BKP(clave,nombre,alias,descripcion,creditos,medias_horas_Sem,nivel) VALUES(@ID,@NAME,@NICK,@DESC,@CRED,@HRS,@LVL) 
END
GO

IF OBJECT_ID('tg_bkp_ALUMNO') IS NOT NULL
	DROP TRIGGER tg_bkp_ALUMNO
GO
CREATE TRIGGER tg_bkp_ALUMNO ON ALUMNO AFTER DELETE,UPDATE AS
BEGIN
	DECLARE @MATR INT;
	DECLARE @NAME VARCHAR(25);
	DECLARE @SNDN VARCHAR(25);
	DECLARE @LSTN VARCHAR(25);
	DECLARE @GEND	BIT;
	DECLARE @FNAC	DATE;
	DECLARE @CARR INT;
	DECLARE @COUN INT;
	SELECT @MATR=matricula,@NAME=nombre,@SNDN=apellido_Pat,@LSTN=apellido_Mat,@GEND=genero,@FNAC=fecha_Naci,@CARR=carrera_fk,@COUN=pais_fk FROM deleted
	INSERT INTO ALUMNO_BKP(matricula,nombre,apellido_Pat,apellido_Mat,genero,fecha_Naci,carrera_fk,pais_fk) VALUES(@MATR,@NAME,@SNDN,@LSTN,@GEND,@FNAC,@CARR,@COUN) 
END
GO

IF OBJECT_ID('tg_bkp_MAESTRO') IS NOT NULL
	DROP TRIGGER tg_bkp_MAESTRO
GO
CREATE TRIGGER tg_bkp_MAESTRO ON MAESTRO AFTER DELETE,UPDATE AS
BEGIN
	DECLARE @MATR INT;
	DECLARE @NUMB VARCHAR(25);
	DECLARE @NAME VARCHAR(25);
	DECLARE @SNDN VARCHAR(25);
	DECLARE @LSTN VARCHAR(25);
	DECLARE @TITLE VARCHAR(50);
	DECLARE @GEND	BIT;
	DECLARE @FNAC	DATE;
	SELECT @MATR=matricula,@NUMB=num_nomina,@NAME=nombre,@SNDN=apellido_Pat,@LSTN=apellido_Mat,@TITLE=titulo,@GEND=genero,@FNAC=fecha_Naci FROM deleted
	INSERT INTO MAESTRO_BKP(matricula,num_nomina,nombre,apellido_Pat,apellido_Mat,titulo,genero,fecha_Naci) VALUES(@MATR,@NUMB,@NAME,@SNDN,@LSTN,@TITLE,@GEND,@FNAC) 
END
GO

IF OBJECT_ID('tg_bkp_AULA') IS NOT NULL
	DROP TRIGGER tg_bkp_AULA
GO
CREATE TRIGGER tg_bkp_AULA ON AULA AFTER DELETE,UPDATE AS
BEGIN
	DECLARE @ID INT;
	DECLARE @CUPO INT;
	DECLARE @LOCA VARCHAR(255);
	SELECT @ID=id,@LOCA=localizacion,@CUPO=cupo FROM deleted
	INSERT INTO AULA_BKP(id,localizacion,cupo) VALUES(@ID,@LOCA,@CUPO) 
END
GO

IF OBJECT_ID('tg_bkp_GRUPO') IS NOT NULL
	DROP TRIGGER tg_bkp_GRUPO
GO
CREATE TRIGGER tg_bkp_GRUPO ON GRUPO AFTER DELETE,UPDATE AS
BEGIN
	DECLARE @ID INT;
	DECLARE @SUBJ INT;
	DECLARE @TCHR INT;
	DECLARE @CLSR INT;
	DECLARE @SEME INT;
	SELECT @ID=id,@SUBJ=materia_fk,@TCHR=maestro_fk,@CLSR=aula_fk,@SEME=semestre_fk FROM deleted
	INSERT INTO GRUPO_BKP(id,materia_fk,maestro_fk,aula_fk,semestre_fk) VALUES(@ID,@SUBJ,@TCHR,@CLSR,@SEME) 
END
GO

IF OBJECT_ID('tg_bkp_ALUMNO_GRUPO') IS NOT NULL
	DROP TRIGGER tg_bkp_ALUMNO_GRUPO
GO
CREATE TRIGGER tg_bkp_ALUMNO_GRUPO ON ALUMNO_GRUPO AFTER DELETE,UPDATE AS
BEGIN
	DECLARE @ID INT;
	DECLARE @CALI FLOAT;
	DECLARE @DATE DATE;
	DECLARE @GRP INT;
	DECLARE @STDN INT;
	
	SELECT @ID=id,@CALI=calificacion,@DATE=fecha_calif,@GRP=grupo_fk,@STDN=alumno_fk FROM deleted
	INSERT INTO ALUMNO_GRUPO_BKP(id,calificacion,fecha_calif,grupo_fk,alumno_fk) VALUES(@ID,@CALI,@DATE,@GRP,@STDN) 
END
GO