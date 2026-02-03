<template>
  <div class="recipes">
    <el-card>
      <template #header>
        <div class="card-header">
          <span>食谱管理</span>
          <el-button type="primary" @click="$router.push('/recipes/create')">添加食谱</el-button>
        </div>
      </template>

      <el-table :data="recipes" stripe>
        <el-table-column label="图片" width="100">
          <template #default="{ row }">
            <el-image
              v-if="row.imageUrl"
              :src="row.imageUrl"
              style="width: 60px; height: 60px"
              fit="cover"
            />
            <el-icon v-else :size="40"><Picture /></el-icon>
          </template>
        </el-table-column>
        <el-table-column prop="name" label="名称" />
        <el-table-column prop="mealType" label="餐次" width="100" />
        <el-table-column prop="description" label="描述" show-overflow-tooltip />
        <el-table-column label="操作" width="200">
          <template #default="{ row }">
            <el-button type="primary" link @click="viewDetail(row)">详情</el-button>
            <el-button type="primary" link @click="editRecipe(row.id)">编辑</el-button>
            <el-button type="danger" link @click="handleDelete(row.id)">删除</el-button>
          </template>
        </el-table-column>
      </el-table>
    </el-card>

    <el-dialog v-model="showDetail" title="食谱详情" width="600px">
      <div v-if="currentRecipe">
        <el-image
          v-if="currentRecipe.imageUrl"
          :src="currentRecipe.imageUrl"
          style="width: 100%; height: 200px"
          fit="cover"
        />
        <h3>{{ currentRecipe.name }}</h3>
        <p>餐次: {{ currentRecipe.mealType }}</p>
        <p>描述: {{ currentRecipe.description }}</p>
        <el-divider content-position="left">步骤</el-divider>
        <div v-for="step in currentRecipe.steps" :key="step.id" class="step-item">
          <div class="step-header">
            <strong>步骤 {{ step.stepOrder }}</strong>
          </div>
          <el-image
            v-if="step.imageUrl"
            :src="step.imageUrl"
            class="step-image"
            fit="cover"
          />
          <p>{{ step.description }}</p>
          <div v-if="step.ingredients.length">
            <small>食材: {{ step.ingredients.map((i: any) => `${i.ingredientName}${i.amount}${i.unit || ''}`).join(', ') }}</small>
          </div>
          <div v-if="step.condiments.length">
            <small>调味品: {{ step.condiments.map((c: any) => `${c.condimentName}${c.amount}${c.unit || ''}`).join(', ') }}</small>
          </div>
        </div>
      </div>
    </el-dialog>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { ElMessage, ElMessageBox } from 'element-plus'
import { getRecipes, deleteRecipe as apiDeleteRecipe, getRecipeDetail } from '@/api'

const router = useRouter()
const recipes = ref<any[]>([])
const showDetail = ref(false)
const currentRecipe = ref<any>(null)

const loadData = async () => {
  const { data } = await getRecipes()
  recipes.value = data
}

const viewDetail = async (row: any) => {
  const { data } = await getRecipeDetail(row.id)
  currentRecipe.value = data
  showDetail.value = true
}

const editRecipe = (id: number) => {
  router.push(`/recipes/${id}/edit`)
}

const handleDelete = async (id: number) => {
  try {
    await ElMessageBox.confirm('确定要删除此食谱吗？', '提示', { type: 'warning' })
    await apiDeleteRecipe(id)
    ElMessage.success('删除成功')
    loadData()
  } catch (error) {
    // User cancelled
  }
}

onMounted(loadData)
</script>

<style scoped>
.card-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
}
.step-item {
  margin-bottom: 20px;
  padding: 10px;
  background: #f5f7fa;
  border-radius: 8px;
}
.step-header {
  margin-bottom: 8px;
}
.step-image {
  width: 200px;
  height: 150px;
  margin: 10px 0;
  border-radius: 4px;
}
</style>
