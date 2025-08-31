CREATE TABLE Companies (
    company_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    name VARCHAR(255) NOT NULL,
    email VARCHAR(255) UNIQUE NOT NULL,
    img TEXT
);

INSERT INTO Companies 
    (name, email, img) 
VALUES
    ('Weyland-Yutani', 'info@wy.com', 'wy.webp'),
    ('Omni Consumer Products', 'info@ocp.com', 'ocp.webp'),
    ('Cyberdyne Systems Corporation', 'info@csc.com', 'csc.webp'),
    ('Umbrella Corporation', 'info@uc.com', 'uc.webp');
    
SELECT * FROM Companies;

CREATE TABLE Products (
    product_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    name VARCHAR(255) NOT NULL,
    description TEXT NOT NULL,
    price INT NOT NULL
);

INSERT INTO Products 
    (name, description, price) 
VALUES
    ('R�plica del Cargador de Montacargas P-5000', 'Un juguete funcional a escala de 1:12 con brazos m�viles y pinzas. Perfecto para recrear la batalla �pica de la Reina Alien.', 55000),
    ('Set de Miniaturas Coleccionables de Xenomorfos', 'Un paquete de tres figuras de 5 cm de un Facehugger, un Chestburster y un Guerrero Xenomorfo. Ideales para exhibici�n en escritorio.', 18000),
    ('Figura de Acci�n de un Sint�tico con Da�os de Batalla', 'Un androide Weyland-Yutani de 15 cm con una funci�n de "da�o" removible que revela el endoesqueleto interior.', 29990),
    ('Veh�culo de Patrulla OCP con Luces y Sonido', 'Un coche de polic�a a escala con el logo de OCP, luces parpadeantes y sonidos de sirena.', 22000),
    ('Figura de RoboCop "Ultimate Edition"', 'Una figura de 18 cm con m�ltiples puntos de articulaci�n, tres caras intercambiables y su arma "Auto 9".', 45000),
    ('Set de Construcci�n "Torre ED-209"', 'Un kit de bloques que te permite armar tu propio robot ED-209. Incluye un mecanismo de disparo de misiles a escala.', 68000),
    ('Figura de Acci�n del T-800 Endoskeleton', 'Una figura de 15 cm completamente cromada, con articulaciones de alta calidad y ojos que se iluminan con un bot�n.', 35000),
    ('Cabeza de Colecci�n de un T-1000 de Metal L�quido', 'Una r�plica a escala 1:1 de la cabeza del T-1000, con una apariencia de metal l�quido plateado y detalles de da�os.', 75000),
    ('R�plica del Computador de Skynet', 'Un juguete interactivo con forma de la ic�nica computadora, que simula el proceso de inicio de un apocalipsis en miniatura.', 50000),
    ('Figura de Acci�n del T-Virus Zombie', 'Una figura de un zombi con un efecto de "piel podrida" y un recipiente con un l�quido simulando el T-Virus que brilla en la oscuridad.', 28000),
    ('Set de Laboratorio Secreto de Umbrella', 'Un playset a peque�a escala de un laboratorio, con tubos de ensayo, un microscopio y una peque�a figura del Birkin G-Virus.', 60000),
    ('R�plica de la Vacuna Anti-Virus', 'Una ampolla de vidrio de colecci�n que contiene un l�quido de color azul, dise�ado para parecerse a la cura de la pel�cula.', 15000);

SELECT * FROM Products;
