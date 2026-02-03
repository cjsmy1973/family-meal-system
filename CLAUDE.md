# 家庭点餐系统 (Family Meal System)

全栈家庭点餐应用，包含食材管理、调味品管理、食谱管理和预约功能。

## 技术栈

- **前端**: Vue 3 + Element Plus + TypeScript + Vite
- **后端**: ASP.NET Core 8 Web API
- **数据库**: MySQL 8.0
- **部署**: Docker + Docker Compose

## 项目结构

```
family-meal-system/
├── docker-compose.yml          # Docker Compose 配置
├── backend/
│   ├── FamilyMeal.Api/
│   │   ├── Controllers/        # API 控制器
│   │   │   ├── IngredientsController.cs
│   │   │   ├── CondimentsController.cs
│   │   │   ├── RecipesController.cs
│   │   │   ├── ReservationsController.cs
│   │   │   └── UploadController.cs
│   │   ├── Models/             # 实体和 DTO
│   │   │   ├── Ingredient.cs, Condiment.cs
│   │   │   ├── Recipe.cs, RecipeStep.cs
│   │   │   ├── StepIngredient.cs, StepCondiment.cs
│   │   │   ├── Reservation.cs
│   │   │   └── Dtos.cs
│   │   ├── Services/           # 业务逻辑层
│   │   │   ├── IngredientService.cs
│   │   │   ├── CondimentService.cs
│   │   │   ├── RecipeService.cs
│   │   │   └── ReservationService.cs
│   │   ├── Data/               # 数据库上下文
│   │   │   └── FamilyMealDbContext.cs
│   │   ├── Program.cs
│   │   └── appsettings.json
│   └── FamilyMeal.sln
└── frontend/
    ├── src/
    │   ├── api/                # API 封装
    │   │   └── index.ts
    │   ├── views/              # 页面组件
    │   │   ├── Home.vue
    │   │   ├── Ingredients.vue
    │   │   ├── Condiments.vue
    │   │   ├── Recipes.vue
    │   │   ├── RecipeForm.vue
    │   │   └── Reservations.vue
    │   ├── router/             # 路由配置
    │   ├── App.vue
    │   ├── main.ts
    │   └── style.css
    ├── package.json
    ├── vite.config.ts
    └── nginx.conf
```

## 数据库设计

### 核心表
- `Ingredients` - 食材表
- `Condiments` - 调味品表
- `Recipes` - 食谱表
- `RecipeSteps` - 食谱步骤表
- `StepIngredients` - 步骤食材关联表
- `StepCondiments` - 步骤调味品关联表
- `Reservations` - 预约表

## API 端点

| 端点 | 方法 | 描述 |
|------|------|------|
| `/api/ingredients` | GET/POST | 食材列表/创建 |
| `/api/ingredients/{id}` | PUT/DELETE | 食材编辑/删除 |
| `/api/condiments` | GET/POST | 调味品列表/创建 |
| `/api/condiments/{id}` | PUT/DELETE | 调味品编辑/删除 |
| `/api/recipes` | GET/POST | 食谱列表/创建 |
| `/api/recipes/{id}` | PUT/DELETE | 食谱编辑/删除 |
| `/api/recipes/{id}/detail` | GET | 食谱详情（含步骤） |
| `/api/reservations` | GET/POST | 预约列表/创建 |
| `/api/reservations/shopping-list` | GET | 生成购物清单 |
| `/api/reservations/{id}` | DELETE | 取消预约 |
| `/api/upload` | POST | 图片上传 |

## 开发命令

### 后端
```bash
cd backend/FamilyMeal.Api
dotnet restore
dotnet run
```

### 前端
```bash
cd frontend
npm install
npm run dev
```

### Docker 部署
```bash
docker-compose up -d --build
```

## 端口配置

| 服务 | 端口 |
|------|------|
| 前端 | 3000 |
| 后端 API | 5000 |
| MySQL | 3306 |

## 注意事项

- 后端需要 MySQL 数据库连接
- 配置文件使用 `appsettings.json` 中的连接字符串
- Docker 部署时使用 `docker-compose.yml` 环境变量
- 前端 nginx 代理 `/api` 请求到后端
