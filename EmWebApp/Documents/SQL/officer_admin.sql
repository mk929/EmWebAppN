
/* Password: 111111 */
INSERT INTO [dbo].[AspNetUsers]
           ([Id]
           ,[Email]
           ,[EmailConfirmed]
           ,[PasswordHash]
           ,[SecurityStamp]
           ,[PhoneNumber]
           ,[PhoneNumberConfirmed]
           ,[TwoFactorEnabled]
           ,[LockoutEndDateUtc]
           ,[LockoutEnabled]
           ,[AccessFailedCount]
           ,[UserName])
     VALUES
           ('e54ef090-6948-4564-81a2-a0dc35eaa4f9'
           ,'officer@admin.com'
           ,0
           ,'AAMppczm2lEY0byBPyL9boZiwR7rmfGojHHB6aIwdABZsj6K4blgnKkcK1pVl/6FzA=='
           ,'d87a6f69-4ad5-4677-a470-8f15d6970117'
           ,null
           ,0
           ,0
           ,null
           ,1
           ,0
           ,'officer@admin.com')
GO