﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PeliculasAPI.Migrations
{
    /// <inheritdoc />
    public partial class IdentitySeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            BEGIN TRANSACTION;
            IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ConcurrencyStamp', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[AspNetRoles]'))
                SET IDENTITY_INSERT [AspNetRoles] ON;
            INSERT INTO [AspNetRoles] ([Id], [ConcurrencyStamp], [Name], [NormalizedName])
            VALUES (N'9aae0b6d-d50c-4d0a-9b90-2a6873e3845d', N'83f333e5-4024-4eb1-8355-483b1714d1e5', N'Admin', N'Admin');
            IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ConcurrencyStamp', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[AspNetRoles]'))
                SET IDENTITY_INSERT [AspNetRoles] OFF;

            IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AccessFailedCount', N'ConcurrencyStamp', N'Email', N'EmailConfirmed', N'LockoutEnabled', N'LockoutEnd', N'NormalizedEmail', N'NormalizedUserName', N'PasswordHash', N'PhoneNumber', N'PhoneNumberConfirmed', N'SecurityStamp', N'TwoFactorEnabled', N'UserName') AND [object_id] = OBJECT_ID(N'[AspNetUsers]'))
                SET IDENTITY_INSERT [AspNetUsers] ON;
            INSERT INTO [AspNetUsers] ([Id], [AccessFailedCount], [ConcurrencyStamp], [Email], [EmailConfirmed], [LockoutEnabled], [LockoutEnd], [NormalizedEmail], [NormalizedUserName], [PasswordHash], [PhoneNumber], [PhoneNumberConfirmed], [SecurityStamp], [TwoFactorEnabled], [UserName])
            VALUES (N'5673b8cf-12de-44f6-92ad-fae4a77932ad', 0, N'c2612fbe-57cf-4e06-a257-82d49086538d', N'felipe@hotmail.com', CAST(0 AS bit), CAST(0 AS bit), NULL, N'felipe@hotmail.com', N'felipe@hotmail.com', N'AQAAAAIAAYagAAAAEJorwCrAxsdFR4t6oO4trfkBhFPqaAKgJYj6YuLsxvR/L4EiBsP9XJLCSvF5QeNqjQ==', NULL, CAST(0 AS bit), N'03078827-d58f-4695-960a-1c0d2c6b7ef0', CAST(0 AS bit), N'felipe@hotmail.com');
            IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AccessFailedCount', N'ConcurrencyStamp', N'Email', N'EmailConfirmed', N'LockoutEnabled', N'LockoutEnd', N'NormalizedEmail', N'NormalizedUserName', N'PasswordHash', N'PhoneNumber', N'PhoneNumberConfirmed', N'SecurityStamp', N'TwoFactorEnabled', N'UserName') AND [object_id] = OBJECT_ID(N'[AspNetUsers]'))
                SET IDENTITY_INSERT [AspNetUsers] OFF;

            IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ClaimType', N'ClaimValue', N'UserId') AND [object_id] = OBJECT_ID(N'[AspNetUserClaims]'))
                SET IDENTITY_INSERT [AspNetUserClaims] ON;
            INSERT INTO [AspNetUserClaims] ([Id], [ClaimType], [ClaimValue], [UserId])
            VALUES (1, N'http://schemas.microsoft.com/ws/2008/06/identity/claims/role', N'Admin', N'5673b8cf-12de-44f6-92ad-fae4a77932ad');
            IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ClaimType', N'ClaimValue', N'UserId') AND [object_id] = OBJECT_ID(N'[AspNetUserClaims]'))
                SET IDENTITY_INSERT [AspNetUserClaims] OFF;

            COMMIT;            
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9aae0b6d-d50c-4d0a-9b90-2a6873e3845d");

            migrationBuilder.DeleteData(
                table: "AspNetUserClaims",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5673b8cf-12de-44f6-92ad-fae4a77932ad");
        }
    }
}
