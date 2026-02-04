<template>
  <div class="home">
    <h1>欢迎使用家庭点餐系统</h1>
    <!-- 功能卡片网格 -->
    <el-row :gutter="16" class="feature-grid" style="margin-top: 30px">
      <el-col :xs="12" :sm="12" :md="6" :lg="6" :xl="6">
        <el-card
          shadow="hover"
          class="feature-card"
          @click="$router.push('/ingredients')"
        >
          <div class="feature-icon">
            <el-icon :size="40" color="#409EFF"><Grid /></el-icon>
          </div>
          <p>食材管理</p>
        </el-card>
      </el-col>
      <el-col :xs="12" :sm="12" :md="6" :lg="6" :xl="6">
        <el-card
          shadow="hover"
          class="feature-card"
          @click="$router.push('/condiments')"
        >
          <div class="feature-icon">
            <el-icon :size="40" color="#67C23A"><Sugar /></el-icon>
          </div>
          <p>调味品管理</p>
        </el-card>
      </el-col>
      <el-col :xs="12" :sm="12" :md="6" :lg="6" :xl="6">
        <el-card
          shadow="hover"
          class="feature-card"
          @click="$router.push('/recipes')"
        >
          <div class="feature-icon">
            <el-icon :size="40" color="#E6A23C"><Dish /></el-icon>
          </div>
          <p>食谱管理</p>
        </el-card>
      </el-col>
      <el-col :xs="12" :sm="12" :md="6" :lg="6" :xl="6">
        <el-card
          shadow="hover"
          class="feature-card"
          @click="$router.push('/reservations')"
        >
          <div class="feature-icon">
            <el-icon :size="40" color="#F56C6C"><Calendar /></el-icon>
          </div>
          <p>预约点餐</p>
        </el-card>
      </el-col>
    </el-row>

    <!-- 近期预约 -->
    <el-card class="reservation-card">
      <template #header>
        <div class="card-header">
          <span>近期预约</span>
        </div>
      </template>
      <el-table :data="upcomingReservations" empty-text="暂无预约">
        <el-table-column prop="reservationDate" label="日期" width="120">
          <template #default="{ row }">
            {{ formatDate(row.reservationDate) }}
          </template>
        </el-table-column>
        <el-table-column prop="mealType" label="餐次" width="80" />
        <el-table-column prop="recipeName" label="食谱" />
      </el-table>
    </el-card>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { getReservations } from '@/api'
import { Grid, Sugar, Dish, Calendar } from '@element-plus/icons-vue'

const upcomingReservations = ref<any[]>([])

const formatDate = (date: string) => {
  return new Date(date).toLocaleDateString('zh-CN')
}

onMounted(async () => {
  try {
    const { data } = await getReservations()
    upcomingReservations.value = data.slice(0, 5)
  } catch (error) {
    console.error('获取预约失败', error)
  }
})
</script>

<style scoped>
.home {
  text-align: center;
}

.home h1 {
  color: #303133;
  margin-bottom: 8px;
}

/* 功能卡片样式 */
.feature-card {
  cursor: pointer;
  text-align: center;
  margin-bottom: 16px;
  transition: transform 0.2s, box-shadow 0.2s;
}

.feature-card:hover {
  transform: translateY(-4px);
  box-shadow: 0 8px 16px rgba(0, 0, 0, 0.1);
}

.feature-card:active {
  transform: translateY(-2px);
}

.feature-icon {
  padding: 16px 0;
}

.feature-card p {
  margin-top: 8px;
  font-size: 14px;
  color: #606266;
}

/* 预约卡片样式 */
.reservation-card {
  margin-top: 24px;
}

.card-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

/* ==================== 移动端响应式样式 ==================== */
@media (max-width: 768px) {
  .home h1 {
    font-size: 24px;
  }

  .feature-grid {
    margin-top: 20px !important;
    margin-left: 0 !important;
    margin-right: 0 !important;
  }

  .feature-card {
    margin-bottom: 12px;
  }

  .feature-icon {
    padding: 12px 0;
  }

  .feature-icon .el-icon {
    font-size: 32px;
  }

  .feature-card p {
    font-size: 13px;
    margin-top: 4px;
  }

  .reservation-card {
    margin-top: 20px;
  }
}

/* ==================== 小屏手机优化 ==================== */
@media (max-width: 375px) {
  .home h1 {
    font-size: 20px;
  }

  .feature-icon .el-icon {
    font-size: 28px;
  }

  .feature-card p {
    font-size: 12px;
  }
}

/* ==================== 平板适配 ==================== */
@media (min-width: 769px) and (max-width: 1024px) {
  .feature-grid {
    margin-top: 24px !important;
  }

  .feature-card {
    margin-bottom: 16px;
  }
}
</style>
