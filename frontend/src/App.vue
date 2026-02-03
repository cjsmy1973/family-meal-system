<template>
  <el-container class="app-container">
    <el-aside width="220px">
      <div class="logo">家庭点餐系统</div>
      <el-menu
        :default-active="activeMenu"
        router
        background-color="#304156"
        text-color="#bfcbd9"
        active-text-color="#409EFF"
      >
        <el-menu-item index="/">
          <el-icon><HomeFilled /></el-icon>
          <span>首页</span>
        </el-menu-item>
        <el-menu-item index="/ingredients">
          <el-icon><Grid /></el-icon>
          <span>食材管理</span>
        </el-menu-item>
        <el-menu-item index="/condiments">
          <el-icon><Sugar /></el-icon>
          <span>调味品管理</span>
        </el-menu-item>
        <el-menu-item index="/recipes">
          <el-icon><Dish /></el-icon>
          <span>食谱管理</span>
        </el-menu-item>
        <el-menu-item index="/reservations">
          <el-icon><Calendar /></el-icon>
          <span>预约点餐</span>
        </el-menu-item>
      </el-menu>
    </el-aside>
    <el-main>
      <router-view />
    </el-main>
  </el-container>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { useRoute } from 'vue-router'

const route = useRoute()
const activeMenu = computed(() => route.path)
</script>

<style scoped>
.app-container {
  height: 100vh;
}

.el-aside {
  background-color: #304156;
}

.logo {
  height: 60px;
  line-height: 60px;
  text-align: center;
  color: #fff;
  font-size: 18px;
  font-weight: bold;
  background-color: #263445;
}

.el-main {
  background-color: #f0f2f5;
  padding: 20px;
}
</style>
