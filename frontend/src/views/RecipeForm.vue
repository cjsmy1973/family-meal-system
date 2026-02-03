<template>
  <div class="recipe-form">
    <el-card>
      <template #header>
        <span>{{ isEdit ? '编辑食谱' : '创建食谱' }}</span>
      </template>

      <el-form :model="form" label-width="80px">
        <el-form-item label="名称" required>
          <el-input v-model="form.name" />
        </el-form-item>

        <el-form-item label="餐次" required>
          <el-select v-model="form.mealType">
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
          <el-input v-model="form.description" type="textarea" :rows="3" />
        </el-form-item>

        <el-divider content-position="left">烹饪步骤</el-divider>

        <div v-for="(step, index) in form.steps" :key="index" class="step-item">
          <el-card shadow="never">
            <template #header>
              <span>步骤 {{ index + 1 }}</span>
              <el-button type="danger" link @click="removeStep(index)">删除</el-button>
            </template>

            <el-form-item label="描述" required>
              <el-input v-model="step.description" type="textarea" />
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
                <span>{{ getIngredientName(ing.ingredientId) }}</span>
                <el-input-number v-model="ing.amount" :min="0" size="small" />
                <el-input v-model="ing.unit" placeholder="单位" size="small" style="width: 80px" />
                <el-button type="danger" :icon="Delete" circle size="small" @click="removeIngredientDetail(index, ingIdx)" />
              </div>
              <el-button v-if="step.ingredients.length > step.ingredientDetails.length" size="small" @click="addIngredientDetail(index)">
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
                <span>{{ getCondimentName(cond.condimentId) }}</span>
                <el-input-number v-model="cond.amount" :min="0" size="small" />
                <el-input v-model="cond.unit" placeholder="单位" size="small" style="width: 80px" />
                <el-button type="danger" :icon="Delete" circle size="small" @click="removeCondimentDetail(index, condIdx)" />
              </div>
              <el-button v-if="step.condiments.length > step.condimentDetails.length" size="small" @click="addCondimentDetail(index)">
                添加调味品用量
              </el-button>
            </el-form-item>
          </el-card>
        </div>

        <el-button type="primary" plain @click="addStep">添加步骤</el-button>

        <div style="margin-top: 20px">
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
import { Delete } from '@element-plus/icons-vue'
import { getRecipeDetail, getIngredients, getCondiments, createRecipe, updateRecipe, uploadImage } from '@/api'

const route = useRoute()
const router = useRouter()

const isEdit = computed(() => !!route.params.id)
const allIngredients = ref<any[]>([])
const allCondiments = ref<any[]>([])

const form = ref({
  name: '',
  mealType: '午餐',
  description: '',
  imageUrl: '',
  steps: [
    {
      stepOrder: 1,
      description: '',
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
  try {
    const { data } = await uploadImage(file.raw)
    form.value.imageUrl = data.url
  } catch (error) {
    ElMessage.error('图片上传失败')
  }
}

const addStep = () => {
  form.value.steps.push({
    stepOrder: form.value.steps.length + 1,
    description: '',
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
    steps: form.value.steps.map(s => ({
      stepOrder: s.stepOrder,
      description: s.description,
      ingredients: s.ingredientDetails.map((i: any) => ({
        ingredientId: i.ingredientId,
        amount: i.amount,
        unit: i.unit
      })),
      condiments: s.condimentDetails.map((c: any) => ({
        condimentId: c.condimentId,
        amount: c.amount,
        unit: c.unit
      }))
    }))
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
.step-item {
  margin-bottom: 15px;
}
.ingredient-row {
  display: flex;
  align-items: center;
  gap: 10px;
  margin-top: 8px;
}
.avatar-uploader {
  border: 1px dashed #d9d9d9;
  border-radius: 6px;
  cursor: pointer;
  width: 120px;
  height: 120px;
  display: flex;
  align-items: center;
  justify-content: center;
}
.avatar-uploader:hover {
  border-color: #409EFF;
}
.avatar-uploader-icon {
  font-size: 28px;
  color: #8c939d;
}
.avatar {
  width: 120px;
  height: 120px;
}
</style>
