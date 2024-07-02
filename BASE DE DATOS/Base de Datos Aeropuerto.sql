
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
CREATE DATABASE AeropuertosAmericanos
GO

-------------------------------

USE AeropuertosAmericanos
go
--------------------------------------

-- TABLAS
CREATE TABLE Empleados
(
    UsuLog VARCHAR(8) PRIMARY KEY,
    Contrasena VARCHAR(6) NOT NULL,
    NombreCompleto VARCHAR(60) NOT NULL,
    Labor VARCHAR(20) CHECK (Labor IN ('gerente', 'admin', 'vendedor')) 
)
GO

CREATE TABLE Clientes
(
    IDPasaporte VARCHAR(15) NOT NULL PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    NTarjeta VARCHAR(16) NOT NULL,
    Contasena VARCHAR(6) NOT NULL,
    Activo Bit Not NUll default (1)
  )  
Go

CREATE TABLE Ciudades
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
    ImpuestoLlegada int NOT NULL CHECK (ImpuestoLlegada >= 0),
    ImpuestoPartida  int NOT NULL CHECK (ImpuestoPartida >= 0),
    IDCiudad VARCHAR (6) NOT NULL FOREIGN KEY (IDCiudad) REFERENCES Ciudades(IDCiudad),
        Activo Bit Not NUll default (1)
)
Go

CREATE TABLE Vuelo
(

    IDVuelo varchar (15)  NOT NULL PRIMARY KEY  CHECK (LEN(IDVuelo) = 15 AND IDVuelo LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][A-Za-z][A-Za-z][A-Za-z]'),
    CantidadAsientos INT NOT NULL CHECK ( CantidadAsientos between 100 and 300),
    PrecioVuelo  int  NOT NULL CHECK ( PrecioVuelo > 0),
    FechaHoraSalida DATETIME NOT NULL CHECK ((FechaHoraSalida) > GetDate ()),
    FechaHoraLlegada DATETIME NOT NULL  ,
    IDAeropuertoLlegada VARCHAR (3) NOT NULL FOREIGN KEY (IDAeropuertoLlegada) REFERENCES Aeropuerto(IDAeropuerto), CHECK (FechaHoraLLegada > FechaHoraSalida),
    IDAeropuertoSalida VARCHAR (3) NOT NULL FOREIGN KEY (IDAeropuertoSalida) REFERENCES Aeropuerto (IDAeropuerto),
    
    )
Go

CREATE TABLE Venta
(
    IDVenta INT IDENTITY (1,1) NOT NULL PRIMARY KEY ,     
    Fecha DATE NOT NULL DEFAULT GETDATE(),
    Precio MONEY  NOT NULL CHECK (( Precio)>0),
    UsuLog VARCHAR(8) NOT NULL,
    IDPasaporte VARCHAR(15) NOT NULL,
    IDVuelo varchar (15) NOT NULL,
    FOREIGN KEY (UsuLog) REFERENCES Empleados(UsuLog),
    FOREIGN KEY (IDPasaporte) REFERENCES Clientes(IDPasaporte),
    FOREIGN KEY (IDVuelo) REFERENCES Vuelo(IDVuelo),
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
	
	
	
--creacion de usuario IIS para que el sitio pueda acceder a la bd-------------------------------------------
USE master
GO

CREATE LOGIN [IIS APPPOOL\DefaultAppPool] FROM WINDOWS
GO

USE AeropuertosAmericanos
GO

CREATE USER [IIS APPPOOL\DefaultAppPool] FOR LOGIN [IIS APPPOOL\DefaultAppPool]
GO

GRANT EXECUTE TO [IIS APPPOOL\DefaultAppPool]
GO

	
	

---------------------------------

--PROCEDIMIENTOS ALMACENADOS


--LOGUEO


 Create Procedure LogueoEmpleado
 
@UsuLog varchar(15), 
@Contrasena varchar(6)

AS
Begin
	Select * 
	from Empleados 
	Where Usulog = @UsuLog AND Contrasena = @Contrasena
End
go 					


Create Procedure BuscarEmpleado
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
    @Contrasena VARCHAR(6)
    
AS
BEGIN
IF (EXISTS(SELECT * FROM Clientes WHERE IDPasaporte = @IDpasaporte AND Activo = 0))
BEGIN
UPDATE Clientes
SET Nombre = @Nombre, NTarjeta = @NTarjeta, Contasena = @Contrasena, Activo = 1
WHERE IDPasaporte = @IDpasaporte;
RETURN 1;
        
END

    IF (EXISTS(SELECT * FROM Clientes WHERE IDPasaporte = @IDpasaporte AND Activo = 1))
        RETURN -1
           
    INSERT  Clientes (IDPasaporte, Nombre, NTarjeta, Contasena) 
    VALUES (@IDpasaporte, @Nombre, @NTarjeta, @Contrasena)
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
            -- Verificar si la CIUDAD est  activa

    IF NOT EXISTS (SELECT 1 FROM Ciudades WHERE IDCiudad = @IDCiudad AND Activo = 1)
     BEGIN
       RETURN -1;
    END
    
     -- Verificar si el AEROPUERTO  est  activa
     
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
        RETURN 0;   -- Aeropuerto actualizado y activado con  xito
        end
       
        -- Si el aeropuerto no existe, insertar uno nuevo
             IF (Exists(Select * From Aeropuerto Where @IDAeropuerto= @IDAeropuerto AND Activo = 1))
			return -1
			
		Insert Aeropuerto(IDAeropuerto, Nombre, Direccion,ImpuestoLlegada,ImpuestoPartida,IDCiudad) 
		values (@IDAeropuerto, @Nombre, @Direccion, @ImpuestoLlegada,@ImpuestoPartida,@IDCiudad)
		 RETURN 0;  -- Nuevo aeropuerto insertado con  xito
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
    
    IF NOT EXISTS (SELECT 1 FROM Vuelo WHERE IDVuelo = @IDVuelo)
    BEGIN
    
        IF EXISTS (SELECT 1 FROM Aeropuerto WHERE IDAeropuerto = @IDAeropuertoSalida and Activo= 1) AND
           EXISTS (SELECT 1 FROM Aeropuerto WHERE IDAeropuerto = @IDAeropuertoLlegada and Activo =1 )
        BEGIN
            
            INSERT INTO Vuelo (IDVuelo, CantidadAsientos, PrecioVuelo, FechaHoraSalida, FechaHoraLlegada, IDAeropuertoSalida, IDAeropuertoLlegada)
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
    
       
    INSERT INTO Ciudades (IDCiudad, NombreCiudad , NombrePais ) 
    VALUES (@IDCiudad, @NombreCiudad, @NombrePais)
END
GO
 
CREATE PROCEDURE AltaVenta
    
   @IDVenta INT,
    @Precio MONEY,
    @UsuLog VARCHAR(8),
    @IDPasaporte VARCHAR(15),
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
        IF NOT EXISTS (SELECT 1 FROM Clientes WHERE IDPasaporte = @IDPasaporte AND Activo= 1)
        BEGIN
            
            RETURN -1;
        END
        
        -- Verificar que el vuelo exista
        IF NOT EXISTS (SELECT 1 FROM Vuelo WHERE IDVuelo = @IDVuelo)
        BEGIN
        
            RETURN -1;
        END
        
        -- Insertar la venta
        INSERT  Venta(IDVenta, Precio, UsuLog, IDPasaporte, IDVuelo)
        VALUES ( @IDVenta,@Precio,@UsuLog,@IDPasaporte,@IDVuelo);
        
                RETURN 1; --  xito
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
        IF NOT EXISTS (SELECT 1 FROM Venta WHERE  IDVenta = @IDVenta)
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
					 WHERE Pasaje.IDVenta = ( SELECT IDVenta FROM Venta WHERE IDVuelo = ( SELECT IDVuelo FROM Venta WHERE IDVenta = @IDVenta) ) AND Pasaje.NAsiento = @NAsiento )
                        
        BEGIN 
			RETURN -3 ;
        END
        
        -- Insertar el pasaje
        INSERT INTO Pasaje (IDVenta, NPasaporte, NAsiento) VALUES (@IDVenta, @NPasaporte, @NAsiento);
        
               RETURN 1; --  xito
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
    IF EXISTS ( SELECT * FROM Venta WHERE  IDPasaporte  = @IDpasaporte) 
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
	if (Exists(Select * From Vuelo where IDAeropuertoSalida = @IDAeropuerto) or 
	    Exists(Select * From Vuelo where IDAeropuertoLlegada = @IDAeropuerto))
	BEGIN
        -- Realizar la baja l gica
        UPDATE Aeropuerto SET Activo = 0 WHERE IDAeropuerto= @IDAeropuerto;
        RETURN 1; -- Baja l gica exitosa
    END
	
	IF NOT EXISTS (SELECT * FROM Aeropuerto WHERE IDAeropuerto = @IDAeropuerto)

    RETURN -1;
    END
		
   	-- Baja f sica del aeropuerto - no hay dependencias
	Begin Try
		Delete From Aeropuerto where IDAeropuerto = @IDAeropuerto
		return 1 -- Baja exitosa
	End Try
	Begin Catch
		return -2 -- Error en la eliminaci n
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
        -- Realizar la baja l gica
        UPDATE Ciudades SET Activo = 0 WHERE IDCiudad = @IDCiudad;
        RETURN 1; -- Baja l gica exitosa
     
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
	Select * 	From Empleados	Where UsuLog = @UsuLog
End
go
 
Create Proc BuscarCliente        

    @IDpasaporte VARCHAR
    as
Begin
	Select * From Clientes where IDPasaporte  = @IDpasaporte  AND Activo=1
	End
go

CREATE PROC BuscarTodosLosClientes
@IDPasaporte VARCHAR 
AS
Begin 
select * from Clientes where IDPasaporte=@IDPasaporte;
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
	Select * From Ciudades where IDCiudad= @IDCiudad AND Activo=1
End
go

Create proc BuscarTodasCiudades
 @IDCiudad varchar 
as
begin 
Select * From Ciudades where IDCiudad = @IDCiudad
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
		if (Not Exists(Select * From Clientes Where IDPasaporte = @IDPasaporte AND Activo= 1))
			Begin
				return -1
			end
		Else
			Begin
				Update Clientes
				Set Nombre=@Nombre, NTarjeta=@NTarjeta, @Contrasena=@Contrasena  Where IDPasaporte= @IDPasaporte
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
			if ( Not Exists(Select * From Ciudades Where IDCiudad= @IDCiudad AND Activo= 1))
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
		if (Not Exists(Select * From Ciudades Where IDCiudad= @IDCiudad AND Activo= 1))
			Begin
				return -1
			end
		
			Begin
				Update Ciudades Set IDCiudad=@IDCiudad ,NombreCiudad=@NombreCiudad,NombrePais=@NombrePais
				If (@@ERROR = 0)
					return 1
				Else
					return -2
			end
			end
			
	

go



----------------------------------

-- LISTADOS

Create Procedure ListarVuelos

 As 
Begin
	Select *	From Vuelo
	End
go


Create Procedure ListarAeropuerto

 As 
Begin
	Select * 
	From Aeropuerto	Where Activo = 1

End
go

Create Procedure ListarCiudad

 As 
Begin
	Select * 	From Ciudades	Where Activo = 1
End
go

Create Procedure ListarCliente


 As 
Begin
	Select * 	From Clientes 	Where Activo = 1
End
go

CREATE PROCEDURE ListadoVentasVuelo
  @IDVenta varchar 
AS
   Begin
	Select * From Venta  WHERE IDVuelo = @IDVenta
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


------- INGRESO USUARIOS

--INSERT INTO Empleados (UsuLog, Contrasena, NombreCompleto, Labor)
--VALUES 
--    ('Usuario1', 'Contra', 'Alejo Fernandez', 'gerente'),
--    ('Usuario2', 'Contra', 'Pedro Martinez', 'admin'),
--    ('Usuario3', 'Contra', 'Ramiro Gomez', 'admin');
--GO


-- ---- Creaci n del login y el usuario
CREATE LOGIN Usuario1 WITH PASSWORD = 'Contrasena1';
CREATE USER Usuario1 FOR LOGIN Usuario1;

CREATE LOGIN Usuario2 WITH PASSWORD = 'Contrasena1';
CREATE USER Usuario2 FOR LOGIN Usuario2;

CREATE LOGIN Usuario3 WITH PASSWORD = 'Contrasena3';
CREATE USER Usuario3 FOR LOGIN Usuario3;



--- PERMISOS

GRANT SELECT, INSERT ON Empleados TO Usuario1, Usuario2, Usuario3;
GO

---- INGRESO CLIENTES:

INSERT INTO Clientes(IDPasaporte, Nombre, NTarjeta, Contasena)
VALUES ('123456789101112', 'JUAN SOSA', 'B2345678910111213', 'CCC123');

INSERT INTO Clientes(IDPasaporte, Nombre, NTarjeta, Contasena)
VALUES ('183456789101112', 'SONIA GUITIERRES', 'A2345678910111213', 'AAA123');

INSERT INTO Clientes(IDPasaporte, Nombre, NTarjeta, Contasena)
VALUES ('123976789101112', 'ALFREDO ZUNNINO', 'B2345628910111213', 'BBB123');
GO


	 

-- ---- Creaci n del login y el usuario
CREATE LOGIN Usuario1 WITH PASSWORD = 'Contrasena1';
CREATE USER Usuario1 FOR LOGIN Usuario1;

CREATE LOGIN Usuario2 WITH PASSWORD = 'Contrasena1';
CREATE USER Usuario2 FOR LOGIN Usuario2;

CREATE LOGIN Usuario3 WITH PASSWORD = 'Contrasena3';
CREATE USER Usuario3 FOR LOGIN Usuario3;

---- INGRESO CIUDADES


Insert Ciudades(IDCiudad,NombreCiudad,NombrePais) values ('123456' ,'BUENOSAIRES','ARGENTINA')
GO
Insert Ciudades(IDCiudad,NombreCiudad,NombrePais) values ('234577' ,'RIODEJANEIRO','BRASIL')

Insert Ciudades(IDCiudad,NombreCiudad,NombrePais) values ('345678' ,'MONTEVIDEO','URUGUAY')
GO
