create database ds;
use ds;

create table users(
id int(11) PRIMARY KEY AUTO_INCREMENT,
uname varchar(56),
salt int(6),
password varchar(164));