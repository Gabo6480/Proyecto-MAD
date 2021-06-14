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

IF OBJECT_ID('thisSemester') IS NOT NULL
	DROP FUNCTION thisSemester
GO
CREATE FUNCTION thisSemester()
RETURNS INT AS
BEGIN 
	DECLARE @semester INT;
	SELECT @semester = SEMESTRE.id FROM SEMESTRE WHERE SEMESTRE.estado = 1;
	RETURN @semester;
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

--triggers
IF OBJECT_ID('tg_blockSubjectDelete') IS NOT NULL
	DROP PROC tg_blockSubjectDelete
GO
CREATE TRIGGER tg_blockSubjectDelete ON MATERIA AFTER DELETE AS
BEGIN
	DELETE FROM BLOQUEO_MATERIA WHERE bloque_fk IN(SELECT deleted.clave FROM deleted) OR materia_fk IN(SELECT deleted.clave FROM deleted)
END
GO

 SELECT schemas.name AS SchemaName

,all_objects.name AS TableName ,syscolumns.id AS ColumnId

,syscolumns.name AS ColumnName ,systypes.name AS DataType

,syscolumns.length AS CharacterMaximumLength

--,sysproperties.[value] AS ColumnDescription

,ISNULL(syscomments.TEXT,'No Default') AS ColumnDefault 

,CASE WHEN syscolumns.isnullable = 1 THEN 'NULLABLE' ELSE 'NOT NULL' END AS IsNullable

FROM syscolumns INNER JOIN sys.systypes ON syscolumns.xtype = systypes.xtype

LEFT JOIN sys.all_objects ON syscolumns.id = all_objects.[object_id]

LEFT OUTER JOIN sys.extended_properties AS sysproperties ON (sysproperties.minor_id =

syscolumns.colid AND sysproperties.major_id = syscolumns.id)

LEFT OUTER JOIN sys.syscomments ON syscolumns.cdefault = syscomments.id

LEFT OUTER JOIN sys.schemas ON schemas.[schema_id] = all_objects.[schema_id]

WHERE syscolumns.id IN (SELECT id

FROM sysobjects

WHERE xtype = 'U')

AND (systypes.name <> 'sysname')

ORDER BY TableName ASC, ColumnName ASC