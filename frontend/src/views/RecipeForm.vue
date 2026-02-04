<template>
  <div class="recipe-form">
    <el-card>
      <template #header>
        <div class="card-header">
          <el-button text @click="$router.back()" class="back-btn">
            <el-icon><ArrowLeft /></el-icon>
          </el-button>
          <span class="header-title">{{ isEdit ? '编辑食谱' : '创建食谱' }}</span>
        </div>
      </template>

      <el-form :model="form" label-position="top">
        <el-form-item label="名称" required>
          <el-input v-model="form.name" placeholder="请输入食谱名称" />
        </el-form-item>

        <el-form-item label="餐次" required>
          <el-select v-model="form.mealType" style="width: 100%">
            <el-option label="早餐" value="早餐" />
            <el-option label="午餐" value="午餐" />
            <el-option label="晚餐" value="晚餐" />
          </el-select>
        </el-form-item>

        <el-form-item label="图片">
          <el-upload
            class="avatar-uploader"
            :show-file-list="false"
            :auto-upload="false"
            accept="image/*"
            :on-change="handleImageChange"
          >
            <el-image v-if="form.imageUrl" :src="form.imageUrl" fit="cover" class="avatar" />
            <el-icon v-else class="avatar-uploader-icon"><Plus /></el-icon>
          </el-upload>
        </el-form-item>

        <el-form-item label="描述">
          <el-input v-model="form.description" type="textarea" :rows="3" placeholder="请输入描述" />
        </el-form-item>

        <el-divider content-position="left">烹饪步骤</el-divider>

        <div v-for="(step, index) in form.steps" :key="index" class="step-item">
          <el-card shadow="never" class="step-card">
            <template #header>
              <div class="step-header">
                <span>步骤 {{ index + 1 }}</span>
                <el-button
                  v-if="form.steps.length > 1"
                  type="danger"
                  link
                  size="small"
                  @click="removeStep(index)"
                >
                  删除
                </el-button>
              </div>
            </template>

            <el-form-item label="描述" required>
              <el-input v-model="step.description" type="textarea" :rows="2" placeholder="请输入步骤描述" />
            </el-form-item>

            <el-form-item label="步骤图片">
              <el-upload
                class="step-uploader"
                :show-file-list="false"
                :auto-upload="false"
                accept="image/*"
                :on-change="(file: any) => handleStepImageChange(file, index)"
                :disabled="uploadingStepIndex === index"
              >
                <el-image v-if="step.imageUrl" :src="step.imageUrl" fit="cover" class="step-image" />
                <el-icon v-else class="step-uploader-icon"><Plus /></el-icon>
              </el-upload>
              <el-button
                v-if="step.imageUrl"
                type="danger"
                link
                size="small"
                @click="step.imageUrl = ''"
              >
                删除图片
              </el-button>
              <span v-if="uploadingStepIndex === index" class="uploading-text">上传中...</span>
            </el-form-item>

            <el-form-item label="食材">
              <el-select
                v-model="step.ingredients"
                multiple
                filterable
                remote
                placeholder="搜索食材"
                style="width: 100%"
              >
                <el-option
                  v-for="ing in allIngredients"
                  :key="ing.id"
                  :label="ing.name"
                  :value="ing.id"
                />
              </el-select>
              <div v-for="(ing, ingIdx) in step.ingredientDetails" :key="ingIdx" class="ingredient-row">
                <span class="ingredient-name">{{ getIngredientName(ing.ingredientId) }}</span>
                <div class="ingredient-inputs">
                  <el-input-number v-model="ing.amount" :min="0" size="small" />
                  <el-input v-model="ing.unit" placeholder="单位" size="small" class="unit-input" />
                  <el-button
                    type="danger"
                    :icon="Delete"
                    circle
                    size="small"
                    @click="removeIngredientDetail(index, ingIdx)"
                  />
                </div>
              </div>
              <el-button
                v-if="step.ingredients.length > step.ingredientDetails.length"
                size="small"
                type="primary"
                plain
                @click="addIngredientDetail(index)"
              >
                添加食材用量
              </el-button>
            </el-form-item>

            <el-form-item label="调味品">
              <el-select
                v-model="step.condiments"
                multiple
                filterable
                placeholder="搜索调味品"
                style="width: 100%"
              >
                <el-option
                  v-for="cond in allCondiments"
                  :key="cond.id"
                  :label="cond.name"
                  :value="cond.id"
                />
              </el-select>
              <div v-for="(cond, condIdx) in step.condimentDetails" :key="condIdx" class="ingredient-row">
                <span class="ingredient-name">{{ getCondimentName(cond.condimentId) }}</span>
                <div class="ingredient-inputs">
                  <el-input-number v-model="cond.amount" :min="0" size="small" />
                  <el-input v-model="cond.unit" placeholder="单位" size="small" class="unit-input" />
                  <el-button
                    type="danger"
                    :icon="Delete"
                    circle
                    size="small"
                    @click="removeCondimentDetail(index, condIdx)"
                  />
                </div>
              </div>
              <el-button
                v-if="step.condiments.length > step.condimentDetails.length"
                size="small"
                type="primary"
                plain
                @click="addCondimentDetail(index)"
              >
                添加调味品用量
              </el-button>
            </el-form-item>
          </el-card>
        </div>

        <el-button type="primary" plain @click="addStep" class="add-step-btn">
          <el-icon><Plus /></el-icon>
          添加步骤
        </el-button>

        <div class="form-actions">
          <el-button @click="$router.back()">取消</el-button>
          <el-button type="primary" @click="saveRecipe">保存</el-button>
        </div>
      </el-form>
    </el-card>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, computed } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { ElMessage } from 'element-plus'
import { Delete, Plus, ArrowLeft } from '@element-plus/icons-vue'
import { getRecipeDetail, getIngredients, getCondiments, createRecipe, updateRecipe, uploadImage } from '@/api'

const route = useRoute()
const router = useRouter()

const isEdit = computed(() => !!route.params.id)
const allIngredients = ref<any[]>([])
const allCondiments = ref<any[]>([])
const uploadingStepIndex = ref<number | null>(null)

const form = ref({
  name: '',
  mealType: '午餐',
  description: '',
  imageUrl: '',
  steps: [
    {
      stepOrder: 1,
      description: '',
      imageUrl: '',
      ingredients: [] as number[],
      ingredientDetails: [] as any[],
      condiments: [] as number[],
      condimentDetails: [] as any[]
    }
  ]
})

const getIngredientName = (id: number) => {
  const ing = allIngredients.value.find(i => i.id === id)
  return ing ? ing.name : ''
}

const getCondimentName = (id: number) => {
  const cond = allCondiments.value.find(c => c.id === id)
  return cond ? cond.name : ''
}

const handleImageChange = async (file: any) => {
  uploadingStepIndex.value = -1
  try {
    const { data } = await uploadImage(file.raw)
    form.value.imageUrl = data.url
  } catch (error) {
    ElMessage.error('图片上传失败')
  } finally {
    uploadingStepIndex.value = null
  }
}

const handleStepImageChange = async (file: any, stepIndex: number) => {
  uploadingStepIndex.value = stepIndex
  try {
    const { data } = await uploadImage(file.raw)
    form.value.steps[stepIndex].imageUrl = data.url
    ElMessage.success('图片上传成功')
  } catch (error) {
    ElMessage.error('图片上传失败')
  } finally {
    uploadingStepIndex.value = null
  }
}

const addStep = () => {
  form.value.steps.push({
    stepOrder: form.value.steps.length + 1,
    description: '',
    imageUrl: '',
    ingredients: [],
    ingredientDetails: [],
    condiments: [],
    condimentDetails: []
  })
}

const removeStep = (index: number) => {
  form.value.steps.splice(index, 1)
  form.value.steps.forEach((step, i) => {
    step.stepOrder = i + 1
  })
}

const addIngredientDetail = (stepIndex: number) => {
  const step = form.value.steps[stepIndex]
  const addedIds = step.ingredientDetails.map((d: any) => d.ingredientId)
  const newId = step.ingredients.find((id: number) => !addedIds.includes(id))
  if (newId) {
    step.ingredientDetails.push({ ingredientId: newId, amount: 0, unit: '' })
  }
}

const removeIngredientDetail = (stepIndex: number, ingIndex: number) => {
  form.value.steps[stepIndex].ingredientDetails.splice(ingIndex, 1)
}

const addCondimentDetail = (stepIndex: number) => {
  const step = form.value.steps[stepIndex]
  const addedIds = step.condimentDetails.map((d: any) => d.condimentId)
  const newId = step.condiments.find((id: number) => !addedIds.includes(id))
  if (newId) {
    step.condimentDetails.push({ condimentId: newId, amount: 0, unit: '' })
  }
}

const removeCondimentDetail = (stepIndex: number, condIndex: number) => {
  form.value.steps[stepIndex].condimentDetails.splice(condIndex, 1)
}

const loadData = async () => {
  const [ingRes, condRes] = await Promise.all([getIngredients(), getCondiments()])
  allIngredients.value = ingRes.data
  allCondiments.value = condRes.data

  if (isEdit.value) {
    const { data } = await getRecipeDetail(Number(route.params.id))
    form.value = {
      name: data.name,
      mealType: data.mealType,
      description: data.description || '',
      imageUrl: data.imageUrl || '',
      steps: data.steps.map((s: any) => ({
        stepOrder: s.stepOrder,
        description: s.description,
        imageUrl: s.imageUrl || '',
        ingredients: s.ingredients.map((i: any) => i.ingredientId),
        ingredientDetails: s.ingredients.map((i: any) => ({
          ingredientId: i.ingredientId,
          amount: i.amount,
          unit: i.unit || ''
        })),
        condiments: s.condiments.map((c: any) => c.condimentId),
        condimentDetails: s.condiments.map((c: any) => ({
          condimentId: c.condimentId,
          amount: c.amount,
          unit: c.unit || ''
        }))
      }))
    }
  }
}

const saveRecipe = async () => {
  if (!form.value.name) {
    ElMessage.warning('请输入食谱名称')
    return
  }
  for (const step of form.value.steps) {
    if (!step.description) {
      ElMessage.warning('请填写所有步骤描述')
      return
    }
  }

  const data = {
    name: form.value.name,
    mealType: form.value.mealType,
    description: form.value.description,
    imageUrl: form.value.imageUrl,
    steps: form.value.steps.map(s => {
      let ingredientDetails = s.ingredientDetails
      if (ingredientDetails.length === 0 && s.ingredients.length > 0) {
        ingredientDetails = s.ingredients.map((id: number) => ({
          ingredientId: id,
          amount: 0,
          unit: ''
        }))
      }
      let condimentDetails = s.condimentDetails
      if (condimentDetails.length === 0 && s.condiments.length > 0) {
        condimentDetails = s.condiments.map((id: number) => ({
          condimentId: id,
          amount: 0,
          unit: ''
        }))
      }
      return {
        stepOrder: s.stepOrder,
        description: s.description,
        imageUrl: s.imageUrl,
        ingredients: ingredientDetails.map((i: any) => ({
          ingredientId: i.ingredientId,
          amount: i.amount,
          unit: i.unit
        })),
        condiments: condimentDetails.map((c: any) => ({
          condimentId: c.condimentId,
          amount: c.amount,
          unit: c.unit
        }))
      }
    })
  }

  try {
    if (isEdit.value) {
      await updateRecipe(Number(route.params.id), data)
    } else {
      await createRecipe(data)
    }
    ElMessage.success('保存成功')
    router.push('/recipes')
  } catch (error) {
    ElMessage.error('保存失败')
  }
}

onMounted(loadData)
</script>

<style scoped>
.recipe-form {
  width: 100%;
}

.card-header {
  display: flex;
  align-items: center;
  gap: 12px;
}

.back-btn {
  padding: 8px;
}

.header-title {
  font-size: 18px;
  font-weight: 600;
}

.step-item {
  margin-bottom: 20px;
}

.step-card {
  margin-bottom: 16px;
}

.step-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.ingredient-row {
  display: flex;
  flex-direction: column;
  gap: 8px;
  margin-top: 12px;
  padding: 12px;
  background: #f5f7fa;
  border-radius: 8px;
}

.ingredient-name {
  font-weight: 500;
  color: #606266;
  font-size: 14px;
}

.ingredient-inputs {
  display: flex;
  align-items: center;
  gap: 8px;
  flex-wrap: wrap;
}

.unit-input {
  width: 80px;
}

.uploading-text {
  margin-left: 12px;
  color: #409EFF;
  font-size: 14px;
}

/* 上传组件 */
.avatar-uploader {
  border: 2px dashed #d9d9d9;
  border-radius: 12px;
  cursor: pointer;
  width: 120px;
  height: 120px;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: border-color 0.3s;
}

.avatar-uploader:hover {
  border-color: #409EFF;
}

.avatar-uploader-icon {
  font-size: 32px;
  color: #8c939d;
}

.avatar {
  width: 120px;
  height: 120px;
  border-radius: 12px;
  display: block;
}

.step-uploader {
  border: 2px dashed #d9d9d9;
  border-radius: 8px;
  cursor: pointer;
  width: 100px;
  height: 100px;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: border-color 0.3s;
}

.step-uploader:hover {
  border-color: #409EFF;
}

.step-uploader-icon {
  font-size: 28px;
  color: #8c939d;
}

.step-image {
  width: 100px;
  height: 100px;
  border-radius: 8px;
  display: block;
}

.add-step-btn {
  width: 100%;
  margin: 20px 0;
  height: 48px;
}

.form-actions {
  display: flex;
  gap: 12px;
  margin-top: 24px;
  justify-content: flex-end;
}

.form-actions .el-button {
  min-width: 100px;
  height: 44px;
}

/* ==================== 移动端响应式样式 ==================== */
@media (max-width: 768px) {
  .card-header {
    padding: 8px 0;
  }

  .header-title {
    font-size: 16px;
  }

  .step-card {
    margin-bottom: 12px;
  }

  .step-header {
    font-size: 14px;
  }

  .ingredient-row {
    padding: 10px;
  }

  .ingredient-inputs {
    justify-content: space-between;
  }

  .unit-input {
    width: 70px;
  }

  .avatar-uploader {
    width: 100px;
    height: 100px;
  }

  .avatar {
    width: 100px;
    height: 100px;
  }

  .step-uploader {
    width: 80px;
    height: 80px;
  }

  .step-image {
    width: 80px;
    height: 80px;
  }

  .add-step-btn {
    margin: 16px 0;
  }

  .form-actions {
    flex-direction: column-reverse;
    gap: 12px;
  }

  .form-actions .el-button {
    width: 100%;
  }
}

@media (max-width: 375px) {
  .header-title {
    font-size: 15px;
  }

  .avatar-uploader {
    width: 80px;
    height: 80px;
  }

  .avatar {
    width: 80px;
    height: 80px;
  }

  .avatar-uploader-icon {
    font-size: 24px;
  }

  .step-uploader {
    width: 70px;
    height: 70px;
  }

  .step-image {
    width: 70px;
    height: 70px;
  }

  .step-uploader-icon {
    font-size: 20px;
  }

  .unit-input {
    width: 60px;
  }
}
</style>
