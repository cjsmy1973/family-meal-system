<template>
  <div class="reservations">
    <el-row :gutter="20">
      <el-col :xs="24" :sm="24" :md="14" :lg="14" :xl="14">
        <!-- 预约表单 -->
        <el-card class="reservation-card">
          <template #header>
            <div class="card-header">
              <span class="card-title">预约点餐</span>
            </div>
          </template>

          <el-form :model="reservationForm" label-position="top">
            <el-form-item label="日期" required>
              <el-date-picker
                v-model="reservationForm.reservationDate"
                type="date"
                placeholder="选择日期"
                format="YYYY-MM-DD"
                value-format="YYYY-MM-DD"
                style="width: 100%"
              />
            </el-form-item>

            <el-form-item label="餐次" required>
              <el-select v-model="reservationForm.mealType" style="width: 100%">
                <el-option label="早餐" value="早餐" />
                <el-option label="午餐" value="午餐" />
                <el-option label="晚餐" value="晚餐" />
              </el-select>
            </el-form-item>

            <el-form-item label="食谱" required>
              <el-select v-model="reservationForm.recipeId" placeholder="选择食谱" filterable style="width: 100%">
                <el-option
                  v-for="recipe in recipes"
                  :key="recipe.id"
                  :label="recipe.name"
                  :value="recipe.id"
                />
              </el-select>
            </el-form-item>

            <el-form-item>
              <el-button type="primary" @click="handleCreateReservation" class="full-width-btn">
                添加预约
              </el-button>
            </el-form-item>
          </el-form>
        </el-card>

        <!-- 当前预约 -->
        <el-card class="reservation-list-card">
          <template #header>
            <span class="card-title">当前预约 ({{ reservations.length }})</span>
          </template>

          <el-table :data="reservations" stripe class="responsive-table">
            <el-table-column prop="reservationDate" label="日期" width="100">
              <template #default="{ row }">
                {{ formatDate(row.reservationDate) }}
              </template>
            </el-table-column>
            <el-table-column prop="mealType" label="餐次" width="70" />
            <el-table-column prop="recipeName" label="食谱" />
            <el-table-column label="操作" width="70" fixed="right">
              <template #default="{ row }">
                <el-button type="danger" link @click="handleDeleteReservation(row.id)">取消</el-button>
              </template>
            </el-table-column>
          </el-table>
        </el-card>
      </el-col>

      <!-- 购物清单 -->
      <el-col :xs="24" :sm="24" :md="10" :lg="10" :xl="10">
        <el-card class="shopping-card">
          <template #header>
            <div class="card-header">
              <span class="card-title">购物清单</span>
              <el-button type="primary" link @click="loadShoppingList">
                <el-icon><Refresh /></el-icon>
                <span class="refresh-text">刷新</span>
              </el-button>
            </div>
          </template>

          <div class="date-range">
            <el-form-item label="开始日期">
              <el-date-picker
                v-model="shoppingForm.startDate"
                type="date"
                format="YYYY-MM-DD"
                value-format="YYYY-MM-DD"
                @change="loadShoppingList"
                style="width: 100%"
              />
            </el-form-item>
            <el-form-item label="结束日期">
              <el-date-picker
                v-model="shoppingForm.endDate"
                type="date"
                format="YYYY-MM-DD"
                value-format="YYYY-MM-DD"
                @change="loadShoppingList"
                style="width: 100%"
              />
            </el-form-item>
          </div>

          <el-divider content-position="left">食材清单</el-divider>
          <el-table
            v-if="shoppingList.ingredients.length"
            :data="shoppingList.ingredients"
            size="small"
            class="shopping-table"
          >
            <el-table-column prop="name" label="名称" min-width="100" />
            <el-table-column prop="totalAmount" label="数量" width="90" align="right">
              <template #default="{ row }">
                {{ (row.totalAmount || 0).toFixed(2) }} {{ row.unit || '' }}
              </template>
            </el-table-column>
            <el-table-column prop="category" label="分类" width="80" />
          </el-table>
          <el-empty v-else description="暂无食材" :image-size="60" />

          <el-divider content-position="left">调味品清单</el-divider>
          <el-table
            v-if="shoppingList.condiments.length"
            :data="shoppingList.condiments"
            size="small"
            class="shopping-table"
          >
            <el-table-column prop="name" label="名称" min-width="120" />
            <el-table-column prop="totalAmount" label="数量" width="100" align="right">
              <template #default="{ row }">
                {{ (row.totalAmount || 0).toFixed(2) }} {{ row.unit || '' }}
              </template>
            </el-table-column>
          </el-table>
          <el-empty v-else description="暂无调味品" :image-size="60" />
        </el-card>
      </el-col>
    </el-row>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { ElMessage } from 'element-plus'
import { getReservations, getRecipes, getShoppingList, createReservation as apiCreateReservation, deleteReservation as apiDeleteReservation } from '@/api'
import { Refresh } from '@element-plus/icons-vue'

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
.reservations {
  width: 100%;
}

.el-row {
  margin-left: 0 !important;
  margin-right: 0 !important;
}

.el-col {
  padding-left: 0 !important;
  padding-right: 0 !important;
}

.card-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  flex-wrap: wrap;
  gap: 12px;
}

.card-title {
  font-size: 18px;
  font-weight: 600;
}

.reservation-card,
.reservation-list-card,
.shopping-card {
  margin-bottom: 20px;
}

.full-width-btn {
  width: 100%;
  height: 48px;
}

.responsive-table {
  width: 100%;
}

.date-range {
  margin-bottom: 16px;
}

.refresh-text {
  margin-left: 4px;
}

.shopping-table {
  width: 100%;
}

/* ==================== 移动端响应式样式 ==================== */
@media (max-width: 768px) {
  .reservation-card,
  .reservation-list-card,
  .shopping-card {
    margin-bottom: 16px;
  }

  .card-title {
    font-size: 16px;
  }

  .card-header {
    flex-direction: row;
    justify-content: space-between;
  }

  .full-width-btn {
    height: 44px;
  }

  .date-range {
    display: flex;
    gap: 12px;
  }

  .date-range .el-form-item {
    flex: 1;
    margin-bottom: 0;
  }

  .el-divider {
    margin: 16px 0;
  }
}

@media (max-width: 480px) {
  .card-title {
    font-size: 15px;
  }

  .date-range {
    flex-direction: column;
    gap: 16px;
  }

  .date-range .el-form-item {
    margin-bottom: 0;
  }
}
</style>
