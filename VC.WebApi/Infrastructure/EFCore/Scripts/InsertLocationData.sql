SET IDENTITY_INSERT [VC].[Image] ON 
INSERT [VC].[Image] ([TId], [ImageId], [FileName], [Description], [Uri], [MetaData_Width], [MetaData_Height], [MetaData_Size], [Created], [CreatedBy], [CreatedInNameOf], [LastModified], [LastModifiedBy], [LastModifiedInNameOf]) VALUES (1, N'78c127fa-1edf-4ee9-9fd3-77a0fe493517', N'image1.jpg', N'[{"language":"de","value":"Vintage Club Eingangsbereich"},{"language":"en","value":"Vintage Club Entrance Area"}]', N'/image1.jpg', 400, 600, 12000, CAST(N'2024-09-26T12:49:55.1583436' AS DateTime2), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', CAST(N'2024-09-26T12:49:55.1583436' AS DateTime2), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000')
INSERT [VC].[Image] ([TId], [ImageId], [FileName], [Description], [Uri], [MetaData_Width], [MetaData_Height], [MetaData_Size], [Created], [CreatedBy], [CreatedInNameOf], [LastModified], [LastModifiedBy], [LastModifiedInNameOf]) VALUES (2, N'bb28ce16-ad5f-44be-a957-fe7558a85ff1', N'image2.jpg', N'[{"language":"de","value":"Vintage Club Tanzfl\u00E4che"},{"language":"en","value":"Vintage Club Dance Floor"}]', N'/image2.jpg', 500, 500, 34000, CAST(N'2024-09-26T12:51:34.2170862' AS DateTime2), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', CAST(N'2024-09-26T12:51:34.2170862' AS DateTime2), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000')
INSERT [VC].[Image] ([TId], [ImageId], [FileName], [Description], [Uri], [MetaData_Width], [MetaData_Height], [MetaData_Size], [Created], [CreatedBy], [CreatedInNameOf], [LastModified], [LastModifiedBy], [LastModifiedInNameOf]) VALUES (3, N'efe61d01-e975-46b7-94c5-efeedebf4f7d', N'image3.jpg', N'[{"language":"de","value":"Theke"},{"language":"en","value":"Bar"}]', N'/image3.jpg', 2840, 750, 34000, CAST(N'2024-09-26T12:52:23.5777212' AS DateTime2), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', CAST(N'2024-09-26T12:52:23.5777212' AS DateTime2), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000')
SET IDENTITY_INSERT [VC].[Image] OFF

SET IDENTITY_INSERT [VC].[Location] ON 

INSERT [VC].[Location] ([TId], [LocationId], [Name], [Address_City], [Address_CountryId], [Address_CountryName], [Address_DeliveryInstruction], [Address_State], [Address_Street], [Address_StreetAffix], [Address_StreetNumber], [Address_ZipCode], [Created], [CreatedBy], [CreatedInNameOf], [LastModified], [LastModifiedBy], [LastModifiedInNameOf]) VALUES (1, N'57049650-333e-ef11-9c83-fcb3bc9af2c6', N'[{"language":"de","value":"Vintage Club"},{"language":"en","value":"Vintage Club"}]', N'München', N'78939DEB-333E-EF11-9C83-FCB3BC9AF2C6', N'Deutschland', NULL, N'Bayern', N'Sonnenstr.', NULL, N'12 b', N'80331', CAST(N'2024-09-26T14:47:00.7433333' AS DateTime2), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', CAST(N'2024-09-26T14:47:00.7433333' AS DateTime2), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000')

INSERT [VC].[Location] ([TId], [LocationId], [Name], [Address_City], [Address_CountryId], [Address_CountryName], [Address_DeliveryInstruction], [Address_State], [Address_Street], [Address_StreetAffix], [Address_StreetNumber], [Address_ZipCode], [Created], [CreatedBy], [CreatedInNameOf], [LastModified], [LastModifiedBy], [LastModifiedInNameOf]) VALUES (2, N'2642cb79-24c0-4709-91a9-ee875ae59f93', N'[{"language":"de","value":"Tanzraum"},{"language":"en","value":"Dance Room"}]', N'string', N'DEU', N'Deutschland', N'string', N'string', N'string', N'string', N'string', N'string', CAST(N'2024-09-26T13:03:53.8640337' AS DateTime2), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', CAST(N'2024-09-26T13:03:53.8640337' AS DateTime2), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000')

SET IDENTITY_INSERT [VC].[Location] OFF

SET IDENTITY_INSERT [VC].[LocationCoverImage] ON 

INSERT [VC].[LocationCoverImage] ([TId], [ImageId], [DisplayOrder], [Language], [ScreenSize], [FocusPointX], [FocusPointY], [LocationId]) VALUES (1, N'78c127fa-1edf-4ee9-9fd3-77a0fe493517', 1, N'de', N'mobile', CAST(33.33 AS Decimal(4, 2)), CAST(25.50 AS Decimal(4, 2)), N'2642cb79-24c0-4709-91a9-ee875ae59f93')

INSERT [VC].[LocationCoverImage] ([TId], [ImageId], [DisplayOrder], [Language], [ScreenSize], [FocusPointX], [FocusPointY], [LocationId]) VALUES (2, N'bb28ce16-ad5f-44be-a957-fe7558a85ff1', 2, N'en', N'mobile', CAST(35.75 AS Decimal(4, 2)), CAST(25.00 AS Decimal(4, 2)), N'2642cb79-24c0-4709-91a9-ee875ae59f93')

INSERT [VC].[LocationCoverImage] ([TId], [ImageId], [DisplayOrder], [Language], [ScreenSize], [FocusPointX], [FocusPointY], [LocationId]) VALUES (3, N'efe61d01-e975-46b7-94c5-efeedebf4f7d', NULL, NULL, NULL, NULL, NULL, N'2642cb79-24c0-4709-91a9-ee875ae59f93')

SET IDENTITY_INSERT [VC].[LocationCoverImage] OFF



