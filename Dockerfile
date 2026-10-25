# Stage 1: Restore dependencies & Cache Layer
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS restore-env
WORKDIR /src

# Sao chép file cấu hình giải pháp (.slnx)
COPY *.slnx ./

# Sao chép cấu trúc thư mục của các dự án con để đảm bảo đường dẫn liên kết
COPY src/Core/Core.csproj ./src/Core/
COPY src/Infrastructure/Infrastructure.csproj ./src/Infrastructure/
COPY src/API/API.csproj ./src/API/

# Thực thi restore riêng biệt để khóa cache layer gói NuGet
RUN dotnet restore ./src/API/API.csproj

# Stage 2: Sao chép mã nguồn và Publish ứng dụng
FROM restore-env AS publish-env
COPY src/ ./src/
# Loại bỏ hoàn toàn cờ --no-restore để tránh xung đột file project.assets.json từ Local
RUN dotnet publish ./src/API/API.csproj -c Release -o /app/publish

# Stage 3: Khởi chạy môi trường Runtime tối giản và bảo mật
FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS runtime
WORKDIR /app
COPY --from=publish-env /app/publish .

# Cấu hình biến môi trường tối ưu hóa cho .NET
ENV ASPNETCORE_ENVIRONMENT=Development \
    DOTNET_LOGGING__CONSOLE__DISABLECOLORS=true \
    DOTNET_PRINT_TELEMETRY_MESSAGE=false

# Chuyển sang quyền hạn user thấp (mặc định cổng 8080 trên Alpine của .NET)
USER app
EXPOSE 8080

ENTRYPOINT ["dotnet", "API.dll"]