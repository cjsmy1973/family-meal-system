<template>
  <div class="reservations">
    <el-row :gutter="20">
      <el-col :span="14">
        <el-card>
          <template #header>
            <div class="card-header">
              <span>预约点餐</span>
            </div>
          </template>

          <el-form :model="reservationForm" label-width="80px">
            <el-form-item label="日期" required>
              <el-date-picker
                v-model="reservationForm.reservationDate"
                type="date"
                placeholder="选择日期"
                format="YYYY-MM-DD"
                value-format="YYYY-MM-DD"
              />
            </el-form-item>

            <el-form-item label="餐次" required>
              <el-select v-model="reservationForm.mealType">
                <el-option label="早餐" value="早餐" />
                <el-option label="午餐" value="午餐" />
                <el-option label="晚餐" value="晚餐" />
              </el-select>
            </el-form-item>

            <el-form-item label="食谱" required>
              <el-select v-model="reservationForm.recipeId" placeholder="选择食谱" filterable>
                <el-option
                  v-for="recipe in recipes"
                  :key="recipe.id"
                  :label="recipe.name"
                  :value="recipe.id"
                />
              </el-select>
            </el-form-item>

            <el-form-item>
              <el-button type="primary" @click="handleCreateReservation">添加预约</el-button>
            </el-form-item>
          </el-form>
        </el-card>

        <el-card style="margin-top: 20px">
          <template #header>
            <span>当前预约 ({{ reservations.length }})</span>
          </template>

          <el-table :data="reservations" stripe>
            <el-table-column prop="reservationDate" label="日期" width="120">
              <template #default="{ row }">
                {{ formatDate(row.reservationDate) }}
              </template>
            </el-table-column>
            <el-table-column prop="mealType" label="餐次" width="100" />
            <el-table-column prop="recipeName" label="食谱" />
            <el-table-column label="操作" width="100">
              <template #default="{ row }">
                <el-button type="danger" link @click="handleDeleteReservation(row.id)">取消</el-button>
              </template>
            </el-table-column>
          </el-table>
        </el-card>
      </el-col>

      <el-col :span="10">
        <el-card>
          <template #header>
            <div class="card-header">
              <span>购物清单</span>
              <el-button type="primary" link @click="loadShoppingList">刷新</el-button>
            </div>
          </template>

          <el-form-item label="开始日期">
            <el-date-picker
              v-model="shoppingForm.startDate"
              type="date"
              format="YYYY-MM-DD"
              value-format="YYYY-MM-DD"
              @change="loadShoppingList"
            />
          </el-form-item>
          <el-form-item label="结束日期">
            <el-date-picker
              v-model="shoppingForm.endDate"
              type="date"
              format="YYYY-MM-DD"
              value-format="YYYY-MM-DD"
              @change="loadShoppingList"
            />
          </el-form-item>

          <el-divider content-position="left">食材清单</el-divider>
          <el-table v-if="shoppingList.ingredients.length" :data="shoppingList.ingredients" size="small">
            <el-table-column prop="name" label="名称" />
            <el-table-column prop="totalAmount" label="数量" width="100">
              <template #default="{ row }">
                {{ row.totalAmount.toFixed(2) }} {{ row.unit }}
              </template>
            </el-table-column>
            <el-table-column prop="category" label="分类" width="100" />
          </el-table>
          <el-empty v-else description="暂无食材" />

          <el-divider content-position="left">调味品清单</el-divider>
          <el-table v-if="shoppingList.condiments.length" :data="shoppingList.condiments" size="small">
            <el-table-column prop="name" label="名称" />
            <el-table-column prop="totalAmount" label="数量" width="100">
              <template #default="{ row }">
                {{ row.totalAmount.toFixed(2) }} {{ row.unit }}
              </template>
            </el-table-column>
          </el-table>
          <el-empty v-else description="暂无调味品" />
        </el-card>
      </el-col>
    </el-row>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { ElMessage } from 'element-plus'
import { getReservations, getRecipes, getShoppingList, createReservation as apiCreateReservation, deleteReservation as apiDeleteReservation } from '@/api'

const reservations = ref<any[]>([])
const recipes = ref<any[]>([])
const shoppingList = ref({ ingredients: [] as any[], condiments: [] as any[] })

const reservationForm = ref({
  reservationDate: '',
  mealType: '午餐',
  recipeId: null as number | null
})

const shoppingForm = ref({
  startDate: new Date().toISOString().split('T')[0],
  endDate: new Date(Date.now() + 7 * 24 * 60 * 60 * 1000).toISOString().split('T')[0]
})

const formatDate = (date: string) => {
  return new Date(date).toLocaleDateString('zh-CN')
}

const loadData = async () => {
  const [resRes, recipeRes] = await Promise.all([getReservations(), getRecipes()])
  reservations.value = resRes.data
  recipes.value = recipeRes.data
}

const loadShoppingList = async () => {
  try {
    const { data } = await getShoppingList(shoppingForm.value.startDate, shoppingForm.value.endDate)
    shoppingList.value = data
  } catch (error) {
    console.error('加载购物清单失败', error)
  }
}

const handleCreateReservation = async () => {
  if (!reservationForm.value.reservationDate || !reservationForm.value.recipeId) {
    ElMessage.warning('请填写完整信息')
    return
  }
  try {
    await apiCreateReservation({
      reservationDate: reservationForm.value.reservationDate,
      mealType: reservationForm.value.mealType,
      recipeId: reservationForm.value.recipeId
    })
    ElMessage.success('预约成功')
    reservationForm.value.recipeId = null
    loadData()
    loadShoppingList()
  } catch (error) {
    ElMessage.error('预约失败')
  }
}

const handleDeleteReservation = async (id: number) => {
  try {
    await apiDeleteReservation(id)
    ElMessage.success('取消成功')
    loadData()
    loadShoppingList()
  } catch (error) {
    ElMessage.error('取消失败')
  }
}

onMounted(() => {
  loadData()
  loadShoppingList()
})
</script>

<style scoped>
.card-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
}
</style>
