CREATE TABLE products (
	id int auto_increment,
	barcode varchar(40),
	name varchar(40),
	brand varchar(40),
	image varchar(255),
	volume decimal(10,2) unsigned,
	volume_with_liquid decimal(10,2) unsigned,
	volume_unit varchar(10),
	created timestamp default current_timestamp,
	modified timestamp default current_timestamp,
	primary key(id)
	);

INSERT INTO products (barcode, name, brand, image, volume, volume_with_liquid, volume_unit)
	VALUES 
	('11111', 'Ketchup', 'Felix', null, 500, 500, 'gram'),
	('22222', 'Ketchup', 'Heinz', null, 500, 500, 'gram'),
	('33333', 'Mustard', 'Felix', 'mustard.png', 300, 300, 'gram');
