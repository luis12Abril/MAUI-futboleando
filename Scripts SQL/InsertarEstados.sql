-- Script para insertar Estados de México en la base de datos
-- Ejecuta este script en SQL Server para poblar la tabla ESTADO

USE [db_a85d0b_futboleandobd]
GO

-- Verificar si ya existen estados
IF NOT EXISTS (SELECT 1 FROM ESTADO WHERE Habilitado = 1)
BEGIN
    -- Insertar estados de México
    INSERT INTO ESTADO (Nombre, Habilitado) VALUES
    ('Aguascalientes', 1),
    ('Baja California', 1),
    ('Baja California Sur', 1),
    ('Campeche', 1),
    ('Chiapas', 1),
    ('Chihuahua', 1),
    ('Coahuila', 1),
    ('Colima', 1),
    ('Durango', 1),
    ('Guanajuato', 1),
    ('Guerrero', 1),
    ('Hidalgo', 1),
    ('Jalisco', 1),
    ('México', 1),
    ('Michoacán', 1),
    ('Morelos', 1),
    ('Nayarit', 1),
    ('Nuevo León', 1),
    ('Oaxaca', 1),
    ('Puebla', 1),
    ('Querétaro', 1),
    ('Quintana Roo', 1),
    ('San Luis Potosí', 1),
    ('Sinaloa', 1),
    ('Sonora', 1),
    ('Tabasco', 1),
    ('Tamaulipas', 1),
    ('Tlaxcala', 1),
    ('Veracruz', 1),
    ('Yucatán', 1),
    ('Zacatecas', 1);

    PRINT 'Estados insertados correctamente';
END
ELSE
BEGIN
    PRINT 'Ya existen estados en la base de datos';
END

-- Verificar datos insertados
SELECT COUNT(*) as TotalEstados FROM ESTADO WHERE Habilitado = 1;
SELECT * FROM ESTADO WHERE Habilitado = 1 ORDER BY Nombre;

-- Ejemplo de municipios para Sonora (ID del estado dependerá de tu base de datos)
-- Descomenta y ajusta el IDESTADO según corresponda
/*
DECLARE @IdSonora INT = (SELECT Idestado FROM ESTADO WHERE Nombre = 'Sonora');

INSERT INTO MUNICIPIO (Nombre, Idestado, Habilitado) VALUES
('Hermosillo', @IdSonora, 1),
('Cajeme', @IdSonora, 1),
('Nogales', @IdSonora, 1),
('San Luis Río Colorado', @IdSonora, 1),
('Navojoa', @IdSonora, 1);

PRINT 'Municipios de Sonora insertados';
*/
GO
