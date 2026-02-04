<template>
  <div class="app-container">
    <!-- 移动端遮罩层 -->
    <div
      class="mobile-overlay"
      :class="{ show: isMobileMenuOpen }"
      @click="closeMobileMenu"
    />

    <!-- 移动端顶部栏 -->
    <div class="mobile-header">
      <el-button
        class="hamburger-btn"
        :icon="isMobileMenuOpen ? ArrowLeft : Menu"
        circle
        @click="toggleMobileMenu"
      />
      <span class="mobile-logo">{{ pageTitle }}</span>
    </div>

    <!-- 侧边栏 -->
    <el-aside
      class="sidebar"
      :class="{ 'sidebar--mobile-open': isMobileMenuOpen }"
    >
      <div class="logo desktop-logo">家庭点餐系统</div>
      <el-menu
        :default-active="activeMenu"
        router
        background-color="#304156"
        text-color="#bfcbd9"
        active-text-color="#409EFF"
        :collapse="isMobileMenuOpen"
        @select="closeMobileMenu"
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

    <!-- 主内容区 -->
    <el-main class="main-content">
      <router-view />
    </el-main>

    <!-- 移动端底部导航栏 -->
    <div class="mobile-bottom-nav">
      <div
        class="nav-item"
        :class="{ active: route.path === '/' }"
        @click="navigateTo('/')"
      >
        <el-icon><HomeFilled /></el-icon>
        <span>首页</span>
      </div>
      <div
        class="nav-item"
        :class="{ active: route.path === '/recipes' }"
        @click="navigateTo('/recipes')"
      >
        <el-icon><Dish /></el-icon>
        <span>食谱</span>
      </div>
      <div
        class="nav-item"
        :class="{ active: route.path === '/reservations' }"
        @click="navigateTo('/reservations')"
      >
        <el-icon><Calendar /></el-icon>
        <span>预约</span>
      </div>
      <div
        class="nav-item"
        @click="toggleMobileMenu"
      >
        <el-icon><Grid /></el-icon>
        <span>更多</span>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted, onUnmounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { HomeFilled, Grid, Sugar, Dish, Calendar, Menu, ArrowLeft } from '@element-plus/icons-vue'

const route = useRoute()
const router = useRouter()
const activeMenu = computed(() => route.path)
const isMobileMenuOpen = ref(false)

const navigateTo = (path: string) => {
  router.push(path)
}

const pageTitle = computed(() => {
  const titles: Record<string, string> = {
    '/': '家庭点餐系统',
    '/ingredients': '食材管理',
    '/condiments': '调味品管理',
    '/recipes': '食谱管理',
    '/reservations': '预约点餐'
  }
  return titles[route.path] || '家庭点餐系统'
})

// 检测屏幕宽度
const checkIsMobile = () => {
  return window.innerWidth <= 768
}

const toggleMobileMenu = () => {
  isMobileMenuOpen.value = !isMobileMenuOpen.value
}

const closeMobileMenu = () => {
  if (checkIsMobile()) {
    isMobileMenuOpen.value = false
  }
}

// 监听窗口大小变化
const handleResize = () => {
  if (!checkIsMobile()) {
    isMobileMenuOpen.value = false
  }
}

onMounted(() => {
  window.addEventListener('resize', handleResize)
})

onUnmounted(() => {
  window.removeEventListener('resize', handleResize)
})
</script>

<style scoped>
.app-container {
  height: 100vh;
  display: flex;
}

/* 侧边栏样式 */
.sidebar {
  background-color: #304156;
  position: fixed;
  left: 0;
  top: 0;
  bottom: 0;
  z-index: 1000;
  transition: transform 0.3s ease, width 0.3s ease;
}

.sidebar:not(.sidebar--mobile-open):not(.el-aside--collapse) {
  width: 220px;
}

/* 桌面端Logo */
.desktop-logo {
  display: block;
}

/* 移动端Logo（隐藏） */
.mobile-header .mobile-logo {
  display: none;
}

/* 移动端侧边栏 - 默认隐藏 */
.sidebar:not(.sidebar--mobile-open) {
  transform: translateX(-100%);
}

.sidebar--mobile-open {
  transform: translateX(0);
}

/* 桌面端侧边栏 - 始终显示 */
@media (min-width: 769px) {
  .sidebar {
    transform: translateX(0) !important;
    width: 220px;
  }

  .main-content {
    margin-left: 220px !important;
  }
}

/* 移动端头部 */
.mobile-header {
  display: none;
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  height: 60px;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  z-index: 999;
  align-items: center;
  padding: 0 16px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.15);
}

.mobile-header .hamburger-btn {
  background: rgba(255, 255, 255, 0.2);
  border: none;
  color: white;
}

.mobile-header .mobile-logo {
  display: block;
  color: white;
  font-size: 18px;
  font-weight: bold;
  margin-left: 12px;
}

/* 主内容区 */
.main-content {
  flex: 1;
  background-color: #f0f2f5;
  padding: 20px;
  margin-left: 0;
  transition: margin-left 0.3s ease;
}

/* Logo样式 */
.logo {
  height: 60px;
  line-height: 60px;
  text-align: center;
  color: #fff;
  font-size: 18px;
  font-weight: bold;
  background-color: #263445;
}

/* 侧边栏收起状态 */
.sidebar .el-menu--collapse {
  width: 64px;
}

.sidebar .el-menu--collapse .el-menu-item span,
.sidebar .el-menu--collapse .el-sub-menu__title span {
  display: none;
}

/* ==================== 移动端响应式样式 ==================== */
@media (max-width: 768px) {
  .app-container {
    padding-top: 60px;
  }

  .mobile-header {
    display: flex;
  }

  .main-content {
    padding: 16px;
    margin-left: 0;
  }

  .sidebar {
    width: 260px;
    transform: translateX(-100%);
  }

  .sidebar--mobile-open {
    transform: translateX(0);
    box-shadow: 4px 0 16px rgba(0, 0, 0, 0.2);
  }

  .desktop-logo {
    display: none;
  }
}

/* ==================== 小屏手机优化 ==================== */
@media (max-width: 375px) {
  .main-content {
    padding: 12px;
  }

  .mobile-header {
    height: 56px;
  }

  .mobile-header .mobile-logo {
    font-size: 16px;
  }
}

/* ==================== 平板适配 ==================== */
@media (min-width: 769px) and (max-width: 1024px) {
  .main-content {
    padding: 16px;
  }
}

/* ==================== 移动端底部导航栏 ==================== */
.mobile-bottom-nav {
  display: none;
  position: fixed;
  bottom: 0;
  left: 0;
  right: 0;
  height: 60px;
  background: white;
  box-shadow: 0 -2px 10px rgba(0, 0, 0, 0.1);
  z-index: 998;
  justify-content: space-around;
  align-items: center;
}

.mobile-bottom-nav .nav-item {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  flex: 1;
  height: 100%;
  color: #909399;
  cursor: pointer;
  transition: color 0.3s;
}

.mobile-bottom-nav .nav-item:hover {
  color: #409EFF;
}

.mobile-bottom-nav .nav-item.active {
  color: #409EFF;
  background-color: #f0f7ff;
}

.mobile-bottom-nav .nav-item .el-icon {
  font-size: 20px;
  margin-bottom: 4px;
}

.mobile-bottom-nav .nav-item span {
  font-size: 12px;
}

@media (max-width: 768px) {
  .app-container {
    padding-bottom: 60px;
  }

  .mobile-bottom-nav {
    display: flex;
  }
}
</style>
