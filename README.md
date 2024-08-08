Dependencies
API
    <PackageReference Include="Dapper" Version="2.1.35" />
    <PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="8.0.7" NoWarn="NU1605" />
    <PackageReference Include="Microsoft.IdentityModel.Tokens" Version="8.0.1" />
    <PackageReference Include="Swashbuckle.AspNetCore" Version="6.4.0" />
    <PackageReference Include="System.IdentityModel.Tokens.Jwt" Version="8.0.1" />

    <ProjectReference Include="..\UserManagement.Domain\UserManagement.Domain.csproj" />
    <ProjectReference Include="..\UserManagement.Repository\UserManagement.Repository.csproj" />
DOMAIN
    <PackageReference Include="Microsoft.Data.SqlClient" Version="5.2.1" />
    <PackageReference Include="Microsoft.Extensions.Configuration" Version="8.0.0" />
    
REPOSITORY
    <PackageReference Include="Dapper" Version="2.1.35" />
    
    <ProjectReference Include="..\UserManagement.Domain\UserManagement.Domain.csproj" />

Default Login Account
- Username: Administrator
- Password: P@ssw0rd01

Projects Target Framework: NET8.0

Notes: 
- If you have both x86 and x64 versions of NET SDK installed, make sure to modify the dotnet path in the systems environment variable.
  Put the appropriate version on top of the other.
  e.g. (my machine is 64 bit)
  C:\Program files\dotnet\
  C:\Progream Files\(x86)\dotnet\
- Adjust database connection string accordingly

version 1 | [2024-08-08] | Nelson Chagas
