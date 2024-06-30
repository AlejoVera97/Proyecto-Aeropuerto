
---------------------------------------
USE master
GO
---------------------------------------	

if exists(Select * FROM SysDataBases WHERE name='Ventas')
BEGIN
	DROP DATABASE AeropuertosAmericanos
END
go
---------------------------------------

USE AeropuertosAmericanos
go
--------------------------------------



-- TABLAS
CREATE TABLE Empleado
(
    UsuLog VARCHAR(8) PRIMARY KEY,
    Contrasena VARCHAR(6) NOT NULL,
    NombreCompleto VARCHAR(60) NOT NULL,
    Labor VARCHAR(20) CHECK (Labor IN ('gerente', 'admin', 'vendedor')) 
)
GO

CREATE TABLE Cliente
(
    IDPasaporte VARCHAR(15) NOT NULL PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    NTarjeta VARCHAR(16) NOT NULL,
    Contasena VARCHAR(6) NOT NULL,
    Activo Bit Not NUll default (1)
  )  
Go

CREATE TABLE Ciudad
(
    IDCiudad VARCHAR(6) NOT NULL PRIMARY KEY CHECK (LEN(IDCiudad) = 6 AND IDCiudad LIKE '[A-Z][A-Z][A-Z][A-Z][A-Z][A-Z]'), 
    NombreCiudad VARCHAR(100) NOT NULL,
    NombrePais VARCHAR(100) NOT NULL,
    Activo BIT NOT NULL DEFAULT (1)
)
GO

CREATE TABLE Aeropuerto
(
    IDAeropuerto VARCHAR (3)  NOT NULL PRIMARY KEY CHECK (LEN(IDAeropuerto) = 3 AND IDAeropuerto LIKE  '[A-Z][A-Z][A-Z]'),
    Nombre VARCHAR(100) NOT NULL,
    Direccion VARCHAR(255) NOT NULL,
    ImpuestoLlegada Money NOT NULL CHECK (ImpuestoLlegada >= 0),
    ImpuestoPartida  Money NOT NULL CHECK (ImpuestoPardia >= 0),
    IDCiudad VARCHAR (6) NOT NULL FOREIGN KEY (IDCiudad) REFERENCES Ciudades(IDCiudad),
        Activo Bit Not NUll default (1)
)
Go

CREATE TABLE Vuelos
(

    IDVuelo varchar (15)  NOT NULL PRIMARY KEY  CHECK (LEN(IDVuelo) = 15 AND IDVuelo LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][A-Za-z][A-Za-z][A-Za-z]'),
    CantidadAsientos INT NOT NULL CHECK ( CantidadAsientos between 100 and 300),
    PrecioVuelo  Money  NOT NULL CHECK (( Precio > 0)),
    FechaHoraSalida DATETIME NOT NULL CHECK ((FechaHoraSalida) > GetDate ()),
    FechaHoraLlegada DATETIME NOT NULL,
    IDAeropuertoLlegada VARCHAR (3) NOT NULL FOREIGN KEY (IDAeropuerto) REFERENCES Aeropuerto(IDAeropuerto),
     CHECK (FechaHoraLLegada > FechaHoraSalida),
    IDAeropuertoSalida VARCHAR (3) NOT NULL FOREIGN KEY (IDAeropuerto) REFERENCES Aeropuerto (IDAeropuerto),
    
    )
Go

CREATE TABLE Ventas
(
    IDVenta INT IDENTITY (1,1) NOT NULL PRIMARY KEY ,     
    Fecha DATE NOT NULL DEFAULT GETDATE(),
    Precio MONEY  NOT NULL CHECK (( Precio)>0),
    UsuLog VARCHAR(8) NOT NULL,
    NPasaporte VARCHAR(15) NOT NULL,
    IDVuelo varchar NOT NULL,
    FOREIGN KEY (UsuLog) REFERENCES Empleados(UsuLog),
    FOREIGN KEY (NPasaporte) REFERENCES Clientes(IDPasaporte),
    FOREIGN KEY (IDVuelo) REFERENCES Vuelos(IDVuelo),
  )
  
  
  
GO
  
CREATE TABLE Pasaje
(
    IDVenta INT NOT NULL,
    NPasaporte VARCHAR(15) NOT NULL,
    NAsiento INT NOT NULL CHECK (NAsiento BETWEEN 1 AND 300),
    PRIMARY KEY (IDVenta, NPasaporte),
    FOREIGN KEY (IDVenta) REFERENCES Venta(IDVenta),
    FOREIGN KEY (NPasaporte) REFERENCES Clientes(IDPasaporte),
      
)
Go
	
	
	----- INGRESO USUARIOS

INSERT INTO Empleados (UsuLog, Contrasena, NombreCompleto, Labor)
VALUES ('Usuario1','Contraseña1','Alejo','Gerente'),
		('Usuario2', 'Contraseña2','Pedro','Admin'),
		 ('Usuario3','Contraseña3','Ramiro','Admin'),
		 ('Usuario4','Contraseña4','Ciro','Vendedor'),
		 ('Usuario5','Contraseña5','Rosana','Vendedor'),
		 ('Usuario6','Contraseña6','Diego','Vendedor'),
		 ('Usuario7','Contraseña7','Gimena','Gerente'),
		 ('Usuario8','Contraseña8','Lucia','Admin'),
		 ('Usuario9','Contraseña9','Luis','Vendedor'),
		 ('Usuario10','Contraseña10','Rosina','Admin')


 ---- Creación del login y el usuario
CREATE LOGIN Usuario1 WITH PASSWORD = 'Contraseña1';
CREATE USER Usuario1 FOR LOGIN Usuario1;

CREATE LOGIN Usuario2 WITH PASSWORD = 'Contraseña1';
CREATE USER Usuario2 FOR LOGIN Usuario2;

CREATE LOGIN Usuario3 WITH PASSWORD = 'Contraseña3';
CREATE USER Usuario3 FOR LOGIN Usuario3;

CREATE LOGIN Usuario4 WITH PASSWORD = 'Contraseña4';
CREATE USER Usuario4 FOR LOGIN Usuario4;

CREATE LOGIN Usuario5 WITH PASSWORD = 'Contraseña5';
CREATE USER Usuario5 FOR LOGIN Usuario5;

CREATE LOGIN Usuario6 WITH PASSWORD = 'Contraseña6';
CREATE USER Usuario6 FOR LOGIN Usuario6;

CREATE LOGIN Usuario7 WITH PASSWORD = 'Contraseña7';
CREATE USER Usuario7 FOR LOGIN Usuario7;

CREATE LOGIN Usuario8 WITH PASSWORD = 'Contraseña8';
CREATE USER Usuario8 FOR LOGIN Usuario8;

CREATE LOGIN Usuario9 WITH PASSWORD = 'Contraseña9';
CREATE USER Usuario9 FOR LOGIN Usuario9;

CREATE LOGIN Usuario10 WITH PASSWORD = 'Contraseña10';
CREATE USER Usuario10 FOR LOGIN Usuario10;


-- 4. Otorgar permisos
GRANT SELECT INSERT ON Empleados TO Usuario1,Usuario2,Usuario3,Usuario4,Usuario5,Usuario6,Usuario7,Usuario8,Usuario9,Usuario10;
go  

---------------------------------

--PROCEDIMIENTOS ALMACENADOS


--LOGUEO


 Create Procedure Loguear 
 
@UsuLog varchar(15), 
@Contrasena varchar(6)

AS
Begin
	Select * 
	from Empleado 
	Where Usulog = @UsuLog AND Contrasena = @Contrasena
End
go 					


Create Procedure BuscoEmpleado
 @UsuLog varchar(15) 
 As
Begin
	Select * from Empleados Where Usulog = @UsuLog
End
go

----------------------------------

  
--ALTAS



CREATE PROCEDURE AltaCliente
    @IDpasaporte VARCHAR(15),
    @Nombre VARCHAR(100),
    @NTarjeta VARCHAR(16),
    @Contasena VARCHAR(6)
    
AS
BEGIN
IF (EXISTS(SELECT * FROM Cliente WHERE IDPasaporte = @IDpasaporte AND Activo = 0))
BEGIN
UPDATE Cliente
SET Nombre = @Nombre, NTarjeta = @NTarjeta, Contasena = @Contasena, Activo = 1
WHERE IDPasaporte = @IDpasaporte;
RETURN 1;
        
END

    IF (EXISTS(SELECT * FROM Clientes WHERE IDPasaporte = @IDpasaporte AND Activo = 1))
        RETURN -1
           
    INSERT  Clientes (IDPasaporte, Nombre, NTarjeta, Contasena) 
    VALUES (@IDpasaporte, @Nombre, @NTarjeta, @Contasena)
END
GO

CREATE PROCEDURE AltaAeropuerto

	@IDAeropuerto INT , 
    @Nombre VARCHAR, 
    @Direccion VARCHAR, 
    @ImpuestoLlegada Money,
    @ImpuestoPartida Money,
    @IDCiudad INT
    
	AS
BEGIN
            -- Verificar si la CIUDAD está activa

    IF NOT EXISTS (SELECT 1 FROM Ciudades WHERE IDCiudad = @IDCiudad AND Activo = 1)
     BEGIN
       RETURN -1;
    END
    
     -- Verificar si el AEROPUERTO  está activa
     
    IF NOT EXISTS( SELECT 1 FROM Aeropuerto WHERE IDAeropuerto= @IDAeropuerto  and Activo = 1)
    BEGIN 
    RETURN -2;
    END 
           
         -- Verificar si el aeropeurto exista y este activo
     
       IF EXISTS (SELECT 1 FROM Aeropuerto WHERE @IDAeropuerto = @IDAeropuerto AND Activo = 0)
    BEGIN
        UPDATE Aeropuerto
        SET Nombre=@Nombre , Direccion=@Direccion, ImpuestoLlegada=@ImpuestoLlegada, ImpuestoPartida=@ImpuestoPartida, IDCiudad = @IDCiudad, Activo = 1
        WHERE IDAeropuerto = @IDAeropuerto 
        RETURN 0;   -- Aeropuerto actualizado y activado con éxito
        end
       
        -- Si el aeropuerto no existe, insertar uno nuevo
             IF (Exists(Select * From Aeropuerto Where @IDAeropuerto= @IDAeropuerto AND Activo = 1))
			return -1
			
		Insert Aeropuerto(IDAeropuerto, Nombre, Direccion,ImpuestoLlegada,ImpuestoPartida,IDCiudad) 
		values (@IDAeropuerto, @Nombre, @Direccion, @ImpuestoLlegada,@ImpuestoPartida,@IDCiudad)
		 RETURN 0;  -- Nuevo aeropuerto insertado con éxito
End  

GO

CREATE PROCEDURE AltaVuelo 
    @IDVuelo VARCHAR(15),
    @CantidadAsientos INT,
    @PrecioVuelo MONEY,
    @FechaHoraSalida DATETIME,
    @FechaHoraLlegada DATETIME,
    @IDAeropuertoSalida VARCHAR(3),
    @IDAeropuertoLlegada VARCHAR(3)
AS
BEGIN
    
    IF NOT EXISTS (SELECT 1 FROM Vuelos WHERE IDVuelo = @IDVuelo)
    BEGIN
    
        IF EXISTS (SELECT 1 FROM Aeropuerto WHERE IDAeropuerto = @IDAeropuertoSalida and Activo= 1) AND
           EXISTS (SELECT 1 FROM Aeropuerto WHERE IDAeropuerto = @IDAeropuertoLlegada and Activo =1 )
        BEGIN
            
            INSERT INTO Vuelos (IDVuelo, CantidadAsientos, PrecioVuelo, FechaHoraSalida, FechaHoraLlegada, IDAeropuertoSalida, IDAeropuertoLlegada)
            VALUES (@IDVuelo, @CantidadAsientos, @PrecioVuelo, @FechaHoraSalida, @FechaHoraLlegada, @IDAeropuertoSalida, @IDAeropuertoLlegada);
            
            IF @@ERROR = 0
                RETURN 1;
            ELSE
                RETURN -2;
        END
        ELSE
        BEGIN
    
            RETURN -3;
        END
    END
    ELSE
    BEGIN
       
        RETURN -1;
    END
END;
GO

CREATE PROCEDURE AltaCiudad
    @IDCiudad VARCHAR (15), 
    @NombreCiudad VARCHAR,
    @NombrePais VARCHAR
    
    
AS
BEGIN
    IF (EXISTS(SELECT * FROM Ciudades WHERE IDCiudad = @IDCiudad  AND Activo = 0))
    BEGIN
        UPDATE Ciudades
        SET NombreCiudad  = @NombreCiudad, NombrePais=@NombrePais , Activo = 1
        WHERE IDCiudad = @IDCiudad
        RETURN 1
    END
    IF (EXISTS(SELECT * FROM Ciudades WHERE IDCiudad= @IDCiudad AND Activo = 1))
        RETURN -1
    
       
    INSERT INTO Ciudades  (IDCiudad, NombreCiudad , NombrePais ) 
    VALUES (@IDCiudad, @NombreCiudad, @NombrePais)
END
GO
 
CREATE PROCEDURE AltaVenta
    
   @IDVenta INT,
    @Precio MONEY,
    @UsuLog VARCHAR(8),
    @NPasaporte VARCHAR(15),
    @IDVuelo INT
AS
BEGIN
       BEGIN TRY
             
        -- Verificar que el empleado exista
        IF NOT EXISTS (SELECT 1 FROM Empleados WHERE UsuLog = @UsuLog)
        BEGIN
           
            RETURN -1;
        END
        
        -- Verificar que el cliente exista
        IF NOT EXISTS (SELECT 1 FROM Clientes WHERE IDPasaporte = @NPasaporte AND Activo= 1)
        BEGIN
            
            RETURN -1;
        END
        
        -- Verificar que el vuelo exista
        IF NOT EXISTS (SELECT 1 FROM Vuelos WHERE IDVuelo = @IDVuelo)
        BEGIN
        
            RETURN -1;
        END
        
        -- Insertar la venta
        INSERT  Ventas( Precio, UsuLog, NPasaporte, IDVuelo)
        VALUES ( @IDVenta,@Precio,@UsuLog,@NPasaporte, @Precio,@IDVuelo);
        
                RETURN 1; -- Éxito
    END TRY
    BEGIN CATCH
        
           
        RETURN -1; -- Error
    END CATCH
END
GO

CREATE PROCEDURE AltaPasaje
    @IDVenta INT,
    @NPasaporte VARCHAR,
    @NAsiento INT
   
AS
BEGIN
    
    BEGIN TRY
              
        -- Verificar que la venta asociada existe
        IF NOT EXISTS (SELECT 1 FROM Ventas WHERE  IDVenta = @IDVenta)
        BEGIN
            RETURN -1;
        END
        
        -- Verificar que el cliente asociado existe
        IF NOT EXISTS (SELECT 1 FROM Clientes WHERE IDPasaporte = @NPasaporte AND Activo= 1)
        BEGIN
            RETURN -2;
        END
        
        
	     
         -- Verificar si el asiento ya ha sido vendido 
         IF EXISTS ( SELECT * 
					 FROM Pasaje 
					 WHERE Pasaje.NVenta = ( SELECT IDVenta FROM Ventas WHERE IDVuelo = ( SELECT IDVuelo FROM Ventas WHERE IDVenta = @IDVenta) ) AND Pasaje.NAsiento = @NAsiento )
                        
        BEGIN 
			RETURN -3 ;
        END
        
        -- Insertar el pasaje
        INSERT INTO Pasaje (NVenta, NPasaporte, NAsiento) VALUES (@IDVenta, @NPasaporte, @NAsiento);
        
               RETURN 1; -- Éxito
    END TRY
    BEGIN CATCH
       
        RETURN -1; -- Error
    END CATCH
END
GO



-----------------------------------------
  
  -- BAJAS


CREATE PROCEDURE BajaCliente             
    @IDpasaporte VARCHAR(15) 
AS
BEGIN
    -- Verificar si el cliente existe
    IF NOT EXISTS (SELECT * FROM Clientes WHERE IDPasaporte = @IDpasaporte)
    BEGIN 
    RETURN -1  -- Cliente no encontrado
    END
			
  -- Marcar al cliente como inactivo si tiene ventas asociadas 
    IF EXISTS ( SELECT * FROM Ventas WHERE  NPasaporte  = @IDpasaporte) 
    BEGIN 
    -- baja logica - hay dependencias
    UPDATE Clientes SET Activo = 0 WHERE IDPasaporte = @IDpasaporte;
	RETURN 1;
	END 
	 
	 
	  -- Eliminar al cliente si no tiene pasajes asociados  
   IF EXISTS (SELECT * FROM Pasaje WHERE NPasaporte = @IDpasaporte)
    BEGIN
        RETURN 1; 
    DELETE FROM Clientes WHERE IDPasaporte =@IDpasaporte
    RETURN 0;  -- Cliente eliminado
	END
	
     RETURN -2;  -- No se pudo inactivar o eliminar el cliente 
END
GO

Create Procedure BajaAeropuerto   
 @IDAeropuerto INT


AS
Begin
	-- Verificar si hay vuelos asociados al aeropuerto
	if (Exists(Select * From Vuelos where IDAeropuertoSalida = @IDAeropuerto) or 
	    Exists(Select * From Vuelos where IDAeropuertoLlegada = @IDAeropuerto))
	BEGIN
        -- Realizar la baja lógica
        UPDATE Aeropuerto SET Activo = 0 WHERE IDAeropuerto= @IDAeropuerto;
        RETURN 1; -- Baja lógica exitosa
    END
	
	IF NOT EXISTS (SELECT * FROM Aeropuerto WHERE IDAeropuerto = @IDAeropuerto)

    RETURN -1;
    END
		
   	-- Baja física del aeropuerto - no hay dependencias
	Begin Try
		Delete From Aeropuerto where IDAeropuerto = @IDAeropuerto
		return 1 -- Baja exitosa
	End Try
	Begin Catch
		return -2 -- Error en la eliminación
	End Catch
	
	
	
go

CREATE PROCEDURE BajaCiudad             
    @IDCiudad VARCHAR(15) 
AS    
BEGIN
    
   IF NOT EXISTS( SELECT * FROM  Ciudades WHERE IDCiudad =@IDCiudad)
   Begin
	return -1
	end
    IF  EXISTS (SELECT * FROM Aeropuerto WHERE IDCiudad = @IDCiudad)
   
        BEGIN
        -- Realizar la baja lógica
        UPDATE Ciudades SET Activo = 0 WHERE IDCiudad = @IDCiudad;
        RETURN 1; -- Baja lógica exitosa
     
    RETURN -1;
    END  
            
    DELETE FROM Ciudades WHERE IDCiudad =@IDCiudad
   RETURN -1
   END 	
go
  

-------------------------------------


--BUSCAR 


Create Proc BuscarEmpleados

@UsuLog varchar (8)

As
Begin
	Select * 	From Empleado	Where UsuLog = @UsuLog
End
go
 
Create Proc BuscarCliente        

    @IDpasaporte VARCHAR
    as
Begin
	Select * From Cliente where IDPasaporte  = @IDpasaporte  AND Activo=1
	End
go

CREATE PROC BuscarTodosLosClientes
@IDPasaporte VARCHAR 
AS
Begin 
select * from Cliente where IDPasaporte=@IDPasaporte;
end 
go

Create Proc BuscarAeropuerto       

@IDAeropuerto varchar

  as
Begin
	Select * From Aeropuerto where IDAeropuerto  =@IDAeropuerto AND Activo=1
End
go

Create proc BuscarTodosAeropuertos

@IDAeropuerto varchar 
as
begin 
Select * from Aeropuerto where IDAeropuerto = @IDAeropuerto 
end 
go

Create Proc BuscarCiudad       

@IDCiudad varchar (6)
  as
Begin
	Select * From Ciudad where IDCiudad= @IDCiudad AND Activo=1
End
go

Create proc BuscarTodasLasCiudad
 @IDCiudad varchar 
as
begin 
Select * From Ciudad where IDCiudad = @IDCiudad
end
GO


------------------------

--MODIFICAR

CREATE PROCEDURE ModificarCliente
    @IDPasaporte VARCHAR,
    @Contrasena varchar, 
    @Nombre VARCHAR,
    @NTarjeta VARCHAR

As
Begin
		if (Not Exists(Select * From Cliente Where IDPasaporte = @IDPasaporte AND Activo= 1))
			Begin
				return -1
			end
		Else
			Begin
				Update Cliente
				Set Nombre=@Nombre, NTarjeta=@NTarjeta, Contasena=@Contrasena  Where IDPasaporte= @IDPasaporte
				If (@@ERROR = 0)
					return 1
				Else
					return -2
			End
End
go


CREATE PROCEDURE ModificarAeropuerto
    @IDAeropuerto INT,
    @Nombre VARCHAR,
    @Direccion VARCHAR,
    @ImpuestoLlegada Money,
    @ImpuestoPartida  Money,
    @IDCiudad VARCHAR  
   
As
Begin
		if (Not Exists(Select * From Aeropuerto Where IDAeropuerto= @IDAeropuerto AND Activo= 1))
			Begin
				return -1
			end
			if ( Not Exists(Select * From Ciudad Where IDCiudad= @IDCiudad AND Activo= 1))
			Begin
				return -1
			end
		Else
			Begin
				Update Aeropuerto Set Nombre=@Nombre,Direccion=@Direccion,ImpuestoLlegada=@ImpuestoLlegada,ImpuestoPartida=@ImpuestoPartida, IDCiudad = @IDCiudad
				 Where IDAeropuerto= @IDAeropuerto
				If (@@ERROR = 0)
					return 1
				Else
					return -2
			End
End
go

CREATE PROCEDURE ModificarCiudad
    @IDCiudad VARCHAR,
    @NombreCiudad VARCHAR,
    @NombrePais VARCHAR
   
As
Begin
		if (Not Exists(Select * From Ciudad Where IDCiudad= @IDCiudad AND Activo= 1))
			Begin
				return -1
			end
		
			Begin
				Update Ciudad Set IDCiudad=@IDCiudad ,NombreCiudad=@NombreCiudad,NombrePais=@NombrePais
				If (@@ERROR = 0)
					return 1
				Else
					return -2
			end
			end
			
	

go



----------------------------------

-- LISTADOS

Create Procedure ListadosVuelos

 As 
Begin
	Select *	From Vuelos
	End
go


Create Procedure ListadosAeropuertos

 As 
Begin
	Select * 
	From Aeropuerto	Where Activo = 1

End
go

Create Procedure ListadosCiudad

 As 
Begin
	Select * 	From Ciudad	Where Activo = 1
End
go

Create Procedure ListadosClientes


 As 
Begin
	Select * 	From Cliente 	Where Activo = 1
End
go

CREATE PROCEDURE ListadoVentasVuelo
  @IDVenta varchar 
AS
   Begin
	Select * From Ventas  WHERE IDVuelo = @IDVenta
End
go


CREATE PROCEDURE ListadoPasajeVenta
    @IDventa INT
AS
Begin
	Select * From Pasaje  where  IDVenta = @IDventa
End
go


-----------------------------------


-- DATOS DE PRUEBA

--Insert Empleados (UsuLog, Contrasena, NombreCompleto, Labor )values( 'PEDRO', 'ABC123', 'PEDRO RODRIGUEZ','VENDEDOR')
--Insert Empleados (UsuLog, Contrasena, NombreCompleto, Labor )values( 'JUAN', 'ZXC123', 'JUAN GONZALES','GERENTE')
--Insert Empleados (UsuLog, Contrasena, NombreCompleto, Labor )values( 'ROCIO', 'QWE123', 'ROCIO SOSA','ADMIN')
--go

--Insert Clientes(IDPasaporte,Nombre,NTarjeta,Contasena) Values ('123456789101112131415','JUAN SOSA, 'ABCDE1234567891011, 'CCC123')
--Insert  Clientes(IDPasaporte,Nombre,NTarjeta,Contasena) Values ('123456789101112131415','SONIA GUITIERRES, 'ABCDE1234567891011, 'AAA123')
--Insert  Clientes(IDPasaporte,Nombre,NTarjeta,Contasena) Values ('123456789101112131415','ALFREDO ZUNNINO, 'ABCDE1234567891011, 'BBB123')
--go

--Insert Ciudades(IDCiudad,NombreCiudad,NombrePais) values ('123456' ,'BUENOSAIRES','ARGENTINA')
--Insert Ciudades(IDCiudad,NombreCiudad,NombrePais) values ('234567' ,'RIODEJANEIRO','BRASIL')
--Insert Ciudades(IDCiudad,NombreCiudad,NombrePais) values ('345678' ,'MONTEVIDEO','URUGUAY')

