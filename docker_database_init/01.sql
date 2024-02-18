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
CREATE PROCEDURE GetAllStorechains()
BEGIN
	Select id, name, image, created, modified from storechains order by name;
END$$

CREATE PROCEDURE GetStorechain(in iStorechainId int)
BEGIN
	Select id, name, image, created, modified 
	from storechains 
	where id = iStorechainId
	order by name;
END$$
DELIMITER ;