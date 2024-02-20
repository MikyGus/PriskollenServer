CREATE TABLE storechains (
	id int auto_increment,
	name varchar(50) not null,
	image varchar(255),
	created timestamp default current_timestamp,
	modified timestamp default current_timestamp,

	primary key(id)
);

INSERT INTO storechains (name, image) VALUES
('Ica', null),
('City Gross', null),
('Coop', null);


DELIMITER $$
$$
CREATE PROCEDURE CreateStoreChain(in Name varchar(40),in Image varchar(40))
BEGIN
	INSERT INTO storechains (name, image) 
	VALUES (Name, Image)
	RETURNING id, name, image, created, modified;
END$$

CREATE PROCEDURE GetAllStoreChains()
BEGIN
	Select id, name, image, created, modified from storechains order by name;
END$$

CREATE PROCEDURE GetStoreChain(in iStorechainId int)
BEGIN
	Select id, name, image, created, modified 
	from storechains 
	where id = iStorechainId
	order by name;
END$$

CREATE PROCEDURE UpdateStoreChain(in iId int, in iName varchar(50), in iImage varchar(50))
BEGIN
	UPDATE storechains
	SET name = iName, image = iImage, modified = CURRENT_TIMESTAMP()
	WHERE id = iId;
END$$
DELIMITER ;