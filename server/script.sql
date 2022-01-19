INSERT INTO HostingDB.dbo.Users (username,full_name,password,priority) VALUES
	 (N'test',N'Test Testovic',N'$2a$11$bCWecSThyAffGgXvkJSyYecf2dpN03XKBGTYm5SSaZy1IJxflNDEu',0),
	 (N'jeremija',N'Jeremija Prokic',N'$2a$11$tQeLyTTKYqERY4ZlGRC1BubkuzRWgxtLfY1M7TNhecmix1ySwQ50y',1),
	 (N'korisnik',N'korisnik',N'$2a$11$XNqNlTc2AUotKkMgrEK3KezjveUtO2EwevhqV0m2R0me.v41uMdGy',1);
INSERT INTO HostingDB.dbo.Servers (ip_address,processor,ram_capacity,ssd_capacity) VALUES
	 (N'1.1.1.1',N'Intel i9',4,512),
	 (N'2.2.2.2',N'Intel i9',4,256),
	 (N'3.3.3.3',N'Intel i5',2,128);
INSERT INTO HostingDB.dbo.ReportTypes (name) VALUES
	 (N'Baza podataka'),
	 (N'Email server'),
	 (N'FTP');
INSERT INTO HostingDB.dbo.Reports (created_at,description,ServerID,UserID,ReportTypeID,is_solved) VALUES
	 ('2022-01-18 00:00:00.0000000',N'Neque porro quisquam est qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit...',1,3,1,0),
	 ('2022-01-18 00:00:00.0000000',N'Neque porro quisquam est qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit...',1,3,2,0),
	 ('2022-01-18 00:00:00.0000000',N'Porro quisquam est qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit...',1,3,2,0),
	 ('2022-01-18 00:00:00.0000000',N'Porro quisquam est qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit...',2,3,2,0);
INSERT INTO HostingDB.dbo.UserServers (ServerID,UserID) VALUES
	 (1,3),
	 (2,3);