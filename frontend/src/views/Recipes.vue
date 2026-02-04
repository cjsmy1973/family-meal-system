<template>
  <div class="recipes">
    <el-card>
      <template #header>
        <div class="card-header">
          <span class="card-title">食谱管理</span>
          <el-button type="primary" @click="$router.push('/recipes/create')">
            <el-icon><Plus /></el-icon>
            <span class="btn-text">添加食谱</span>
          </el-button>
        </div>
      </template>

      <el-table :data="recipes" stripe class="responsive-table">
        <el-table-column label="图片" width="70">
          <template #default="{ row }">
            <el-image
              v-if="row.imageUrl"
              :src="row.imageUrl"
              style="width: 50px; height: 50px"
              fit="cover"
              class="recipe-thumbnail"
            />
            <el-icon v-else :size="28"><Picture /></el-icon>
          </template>
        </el-table-column>
        <el-table-column prop="name" label="名称" />
        <el-table-column prop="mealType" label="餐次" width="70" />
        <el-table-column label="操作" width="160" fixed="right">
          <template #default="{ row }">
            <div class="action-buttons">
              <el-button type="primary" link @click="viewDetail(row)">详情</el-button>
              <el-button type="primary" link @click="editRecipe(row.id)">编辑</el-button>
              <el-button type="danger" link @click="handleDelete(row.id)">删除</el-button>
            </div>
          </template>
        </el-table-column>
      </el-table>
    </el-card>

    <el-dialog
      v-model="showDetail"
      title="食谱详情"
      width="95%"
      max-width="600px"
      append-to-body
      destroy-on-close
    >
      <div v-if="currentRecipe" class="recipe-detail">
        <el-image
          v-if="currentRecipe.imageUrl"
          :src="currentRecipe.imageUrl"
          style="width: 100%; height: 200px"
          fit="cover"
          class="detail-image"
        />
        <div class="detail-info">
          <h3>{{ currentRecipe.name }}</h3>
          <p><span class="label">餐次:</span> {{ currentRecipe.mealType }}</p>
          <p v-if="currentRecipe.description"><span class="label">描述:</span> {{ currentRecipe.description }}</p>
        </div>
        <el-divider content-position="left">烹饪步骤</el-divider>
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
          <p class="step-desc">{{ step.description }}</p>
          <div v-if="step.ingredients.length" class="step-materials">
            <small>食材: {{ step.ingredients.map((i: any) => `${i.ingredientName}${i.amount}${i.unit || ''}`).join(', ') }}</small>
          </div>
          <div v-if="step.condiments.length" class="step-materials">
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
import { Plus, Picture } from '@element-plus/icons-vue'

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
.recipes {
  width: 100%;
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

.action-buttons {
  display: flex;
  gap: 4px;
}

.responsive-table {
  width: 100%;
}

.recipe-thumbnail {
  border-radius: 6px;
  display: block;
}

/* 详情弹窗样式 */
.recipe-detail {
  padding: 0 8px;
}

.detail-info {
  margin: 16px 0;
  text-align: left;
}

.detail-info h3 {
  margin-bottom: 12px;
  font-size: 20px;
}

.detail-info p {
  margin: 8px 0;
  line-height: 1.6;
}

.detail-info .label {
  color: #909399;
  margin-right: 8px;
}

.detail-image {
  width: 100%;
  height: 200px;
  border-radius: 12px;
  margin-bottom: 16px;
  display: block;
}

.step-item {
  margin-bottom: 20px;
  padding: 16px;
  background: #f5f7fa;
  border-radius: 12px;
}

.step-header {
  margin-bottom: 12px;
}

.step-image {
  width: 100%;
  max-width: 200px;
  height: 150px;
  margin: 10px 0;
  border-radius: 8px;
}

.step-desc {
  line-height: 1.6;
  margin: 10px 0;
}

.step-materials {
  margin-top: 8px;
  color: #606266;
}

/* ==================== 移动端响应式样式 ==================== */
@media (max-width: 768px) {
  .card-header {
    flex-direction: column;
    align-items: stretch;
  }

  .card-title {
    text-align: center;
    margin-bottom: 8px;
  }

  .el-button {
    width: 100%;
  }

  .btn-text {
    margin-left: 4px;
  }

  .action-buttons {
    flex-wrap: wrap;
    gap: 8px;
  }

  .action-buttons .el-button {
    flex: 1;
    min-width: 44px;
  }

  .recipe-detail {
    padding: 0;
  }

  .detail-info h3 {
    font-size: 18px;
  }

  .step-item {
    padding: 12px;
  }

  .step-image {
    max-width: 100%;
    height: 180px;
  }
}

@media (max-width: 375px) {
  .card-title {
    font-size: 16px;
  }

  .el-button .el-icon {
    font-size: 16px;
  }

  .detail-info h3 {
    font-size: 16px;
  }

  .step-image {
    height: 140px;
  }
}
</style>
