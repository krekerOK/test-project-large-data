CREATE DATABASE Test

GO

USE Test

CREATE TABLE ReferenceData (
	Id INT NOT NULL IDENTITY,
	LookupName NVARCHAR(10),
	CONSTRAINT PK_RererenceData PRIMARY KEY (Id)
)

GO

CREATE TABLE TransactionalData (
	Id INT NOT NULL IDENTITY,
	ReferenceDataId INT NOT NULL,
	DataValue NVARCHAR(5) NOT NULL,
	CONSTRAINT PK_TransactionalData PRIMARY KEY (Id),
	CONSTRAINT FK_TransactionalData_ReferenceData FOREIGN KEY (ReferenceDataId) REFERENCES ReferenceData (Id)
)

GO

USE master