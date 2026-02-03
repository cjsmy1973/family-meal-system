<template>
  <div class="home">
    <h1>欢迎使用家庭点餐系统</h1>
    <el-row :gutter="20" style="margin-top: 30px">
      <el-col :span="6">
        <el-card shadow="hover" @click="$router.push('/ingredients')">
          <el-icon :size="40" color="#409EFF"><Grid /></el-icon>
          <p>食材管理</p>
        </el-card>
      </el-col>
      <el-col :span="6">
        <el-card shadow="hover" @click="$router.push('/condiments')">
          <el-icon :size="40" color="#67C23A">< seasoning /></el-icon>
          <p>调味品管理</p>
        </el-card>
      </el-col>
      <el-col :span="6">
        <el-card shadow="hover" @click="$router.push('/recipes')">
          <el-icon :size="40" color="#E6A23C"><Dish /></el-icon>
          <p>食谱管理</p>
        </el-card>
      </el-col>
      <el-col :span="6">
        <el-card shadow="hover" @click="$router.push('/reservations')">
          <el-icon :size="40" color="#F56C6C"><Calendar /></el-icon>
          <p>预约点餐</p>
        </el-card>
      </el-col>
    </el-row>

    <el-card style="margin-top: 30px">
      <template #header>
        <span>近期预约</span>
      </template>
      <el-table :data="upcomingReservations" empty-text="暂无预约">
        <el-table-column prop="reservationDate" label="日期" width="150">
          <template #default="{ row }">
            {{ formatDate(row.reservationDate) }}
          </template>
        </el-table-column>
        <el-table-column prop="mealType" label="餐次" width="100" />
        <el-table-column prop="recipeName" label="食谱" />
      </el-table>
    </el-card>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { getReservations } from '@/api'

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
}
.el-card {
  cursor: pointer;
  text-align: center;
}
.el-card p {
  margin-top: 10px;
  font-size: 16px;
}
</style>
