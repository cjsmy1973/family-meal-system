<template>
  <div class="ingredients">
    <el-card>
      <template #header>
        <div class="card-header">
          <span class="card-title">食材管理</span>
          <el-button type="primary" @click="openAddDialog">
            <el-icon><Plus /></el-icon>
            <span class="btn-text">添加食材</span>
          </el-button>
        </div>
      </template>

      <el-table :data="ingredients" stripe class="responsive-table">
        <el-table-column prop="name" label="名称" />
        <el-table-column prop="category" label="分类" width="100" />
        <el-table-column prop="unit" label="单位" width="80" />
        <el-table-column prop="createdAt" label="创建时间" width="110">
          <template #default="{ row }">
            {{ formatDate(row.createdAt) }}
          </template>
        </el-table-column>
        <el-table-column label="操作" width="120" fixed="right">
          <template #default="{ row }">
            <div class="action-buttons">
              <el-button type="primary" link @click="editIngredient(row)">编辑</el-button>
              <el-button type="danger" link @click="handleDelete(row.id)">删除</el-button>
            </div>
          </template>
        </el-table-column>
      </el-table>
    </el-card>

    <el-dialog
      v-model="showDialog"
      :title="isEdit ? '编辑食材' : '添加食材'"
      width="90%"
      max-width="400px"
      append-to-body
      destroy-on-close
    >
      <el-form :model="form" label-position="top">
        <el-form-item label="名称" required>
          <el-input v-model="form.name" placeholder="请输入名称" />
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
import { Plus } from '@element-plus/icons-vue'

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

const openAddDialog = () => {
  form.value = { name: '', category: '', unit: '' }
  isEdit.value = false
  editId.value = null
  showDialog.value = true
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
.ingredients {
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
  gap: 8px;
}

/* 响应式表格 */
.responsive-table {
  width: 100%;
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
    flex-direction: row;
    gap: 12px;
  }

  .action-buttons .el-button {
    flex: 1;
  }
}

/* ==================== 小屏手机优化 ==================== */
@media (max-width: 375px) {
  .card-title {
    font-size: 16px;
  }

  .el-button .el-icon {
    font-size: 16px;
  }
}
</style>
