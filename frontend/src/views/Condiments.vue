<template>
  <div class="condiments">
    <el-card>
      <template #header>
        <div class="card-header">
          <span>调味品管理</span>
          <el-button type="primary" @click="showDialog = true">添加调味品</el-button>
        </div>
      </template>

      <el-table :data="condiments" stripe>
        <el-table-column prop="name" label="名称" />
        <el-table-column prop="unit" label="单位" width="100" />
        <el-table-column prop="createdAt" label="创建时间" width="180">
          <template #default="{ row }">
            {{ formatDate(row.createdAt) }}
          </template>
        </el-table-column>
        <el-table-column label="操作" width="150">
          <template #default="{ row }">
            <el-button type="primary" link @click="editCondiment(row)">编辑</el-button>
            <el-button type="danger" link @click="handleDelete(row.id)">删除</el-button>
          </template>
        </el-table-column>
      </el-table>
    </el-card>

    <el-dialog v-model="showDialog" :title="isEdit ? '编辑调味品' : '添加调味品'" width="400px">
      <el-form :model="form" label-width="80px">
        <el-form-item label="名称" required>
          <el-input v-model="form.name" />
        </el-form-item>
        <el-form-item label="单位">
          <el-input v-model="form.unit" placeholder="如：g、ml、勺" />
        </el-form-item>
      </el-form>
      <template #footer>
        <el-button @click="showDialog = false">取消</el-button>
        <el-button type="primary" @click="saveCondiment">确定</el-button>
      </template>
    </el-dialog>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { ElMessage, ElMessageBox } from 'element-plus'
import { getCondiments, createCondiment, updateCondiment, deleteCondiment as apiDeleteCondiment } from '@/api'

const condiments = ref<any[]>([])
const showDialog = ref(false)
const isEdit = ref(false)
const editId = ref<number | null>(null)

const form = ref({
  name: '',
  unit: ''
})

const formatDate = (date: string) => {
  return new Date(date).toLocaleDateString('zh-CN')
}

const loadData = async () => {
  const { data } = await getCondiments()
  condiments.value = data
}

const editCondiment = (row: any) => {
  editId.value = row.id
  form.value = { name: row.name, unit: row.unit || '' }
  isEdit.value = true
  showDialog.value = true
}

const saveCondiment = async () => {
  if (!form.value.name) {
    ElMessage.warning('请输入名称')
    return
  }
  try {
    if (isEdit.value && editId.value) {
      await updateCondiment(editId.value, form.value)
      ElMessage.success('更新成功')
    } else {
      await createCondiment(form.value)
      ElMessage.success('添加成功')
    }
    showDialog.value = false
    form.value = { name: '', unit: '' }
    isEdit.value = false
    editId.value = null
    loadData()
  } catch (error) {
    ElMessage.error('操作失败')
  }
}

const handleDelete = async (id: number) => {
  try {
    await ElMessageBox.confirm('确定要删除此调味品吗？', '提示', { type: 'warning' })
    await apiDeleteCondiment(id)
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
