# 家庭点餐系统

家庭点餐系统，包含食材管理、调味品管理、食谱管理和预约功能的全栈应用。

## 技术栈

- **前端**: Vue 3 + Element Plus + Vite
- **后端**: ASP.NET Core Web API 8
- **数据库**: MySQL 8.0

## 端口配置

| 服务 | 端口 | 描述 |
|------|------|------|
| 前端 | 8080 | Web 界面 |
| 后端 API | 8088 | REST API |
| MySQL | 3306 | 数据库 |

## Docker 部署

### 前置条件

确保已配置 Docker 镜像加速器（在 Docker Desktop 或服务器上）：

```json
{
  "registry-mirrors": [
    "https://docker.m.daocloud.io",
    "https://hub-mirror.c.163.com"
  ]
}
```

### 方式一：飞牛NAS一键部署（推荐）

1. 将整个项目上传到 NAS
2. 在飞牛NAS的 Docker → 编排 中新建编排
3. 复制 `docker-compose.yml` 内容
4. 点击部署

> 注意：NAS 需要能够访问公网拉取基础镜像（mysql:8.0、dotnet:8.0、node:20-alpine、nginx:alpine）

### 方式二：本地构建镜像后部署

适用于网络无法访问 Docker Hub 的情况：

```bash
# 一键构建所有镜像并导出
.\build-images.bat

# 导入到目标服务器/NAS
docker load -i family-meal-backend.tar
docker load -i family-meal-frontend.tar
```

## 开发模式

```bash
# 后端
cd backend
dotnet restore
dotnet run

# 前端
cd frontend
npm install
npm run dev
```

## 访问地址

- 前端: http://localhost:8080
- 后端 API: http://localhost:8088/api

## API 端点

| 端点 | 方法 | 描述 |
|------|------|------|
| `/api/ingredients` | GET/POST | 食材 CRUD |
| `/api/ingredients/{id}` | PUT/DELETE | 食材编辑/删除 |
| `/api/condiments` | GET/POST | 调味品 CRUD |
| `/api/condiments/{id}` | PUT/DELETE | 调味品编辑/删除 |
| `/api/recipes` | GET/POST | 食谱列表/创建 |
| `/api/recipes/{id}` | PUT/DELETE | 食谱编辑/删除 |
| `/api/recipes/{id}/detail` | GET | 食谱详情（含步骤） |
| `/api/reservations` | GET/POST | 预约列表/创建 |
| `/api/reservations/shopping-list` | GET | 生成购物清单 |
| `/api/reservations/{id}` | DELETE | 取消预约 |
| `/api/upload` | POST | 图片上传 |
