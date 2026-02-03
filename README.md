# 家庭点餐系统

家庭点餐系统，包含食材管理、调味品管理、食谱管理和预约功能的全栈应用。

## 技术栈

- **前端**: Vue 3 + Element Plus + Vite
- **后端**: ASP.NET Core Web API 8
- **数据库**: MySQL 8.0

## 快速启动

### Docker 部署

```bash
docker-compose up -d
```

### 开发模式

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

- 前端: http://localhost:3000
- 后端 API: http://localhost:5000

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
