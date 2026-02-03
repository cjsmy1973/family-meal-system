<template>
  <div class="ingredients">
    <el-card>
      <template #header>
        <div class="card-header">
          <span>食材管理</span>
          <el-button type="primary" @click="showDialog = true">添加食材</el-button>
        </div>
      </template>

      <el-table :data="ingredients" stripe>
        <el-table-column prop="name" label="名称" />
        <el-table-column prop="category" label="分类" />
        <el-table-column prop="unit" label="单位" width="100" />
        <el-table-column prop="createdAt" label="创建时间" width="180">
          <template #default="{ row }">
            {{ formatDate(row.createdAt) }}
          </template>
        </el-table-column>
        <el-table-column label="操作" width="150">
          <template #default="{ row }">
            <el-button type="primary" link @click="editIngredient(row)">编辑</el-button>
            <el-button type="danger" link @click="handleDelete(row.id)">删除</el-button>
          </template>
        </el-table-column>
      </el-table>
    </el-card>

    <el-dialog v-model="showDialog" :title="isEdit ? '编辑食材' : '添加食材'" width="400px">
      <el-form :model="form" label-width="80px">
        <el-form-item label="名称" required>
          <el-input v-model="form.name" />
        </el-form-item>
        <el-form-item label="分类">
          <el-input v-model="form.category" placeholder="如：肉类、蔬菜、水产" />
        </el-form-item>
        <el-form-item label="单位">
          <el-input v-model="form.unit" placeholder="如：kg、g、个" />
        </el-form-item>
      </el-form>
      <template #footer>
        <el-button @click="showDialog = false">取消</el-button>
        <el-button type="primary" @click="saveIngredient">确定</el-button>
      </template>
    </el-dialog>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { ElMessage, ElMessageBox } from 'element-plus'
import { getIngredients, createIngredient, updateIngredient, deleteIngredient as apiDeleteIngredient } from '@/api'

const ingredients = ref<any[]>([])
const showDialog = ref(false)
const isEdit = ref(false)
const editId = ref<number | null>(null)

const form = ref({
  name: '',
  category: '',
  unit: ''
})

const formatDate = (date: string) => {
  return new Date(date).toLocaleDateString('zh-CN')
}

const loadData = async () => {
  const { data } = await getIngredients()
  ingredients.value = data
}

const editIngredient = (row: any) => {
  editId.value = row.id
  form.value = { name: row.name, category: row.category || '', unit: row.unit || '' }
  isEdit.value = true
  showDialog.value = true
}

const saveIngredient = async () => {
  if (!form.value.name) {
    ElMessage.warning('请输入名称')
    return
  }
  try {
    if (isEdit.value && editId.value) {
      await updateIngredient(editId.value, form.value)
      ElMessage.success('更新成功')
    } else {
      await createIngredient(form.value)
      ElMessage.success('添加成功')
    }
    showDialog.value = false
    form.value = { name: '', category: '', unit: '' }
    isEdit.value = false
    editId.value = null
    loadData()
  } catch (error) {
    ElMessage.error('操作失败')
  }
}

const handleDelete = async (id: number) => {
  try {
    await ElMessageBox.confirm('确定要删除此食材吗？', '提示', { type: 'warning' })
    await apiDeleteIngredient(id)
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
</style>
