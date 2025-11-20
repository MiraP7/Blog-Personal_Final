import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Layout from './components/Layout';
import HomePage from './pages/HomePage';
import LoginPage from './pages/LoginPage';
import RegisterPage from './pages/RegisterPage';
import CreatePostPage from './pages/CreatePostPage';
import EditPostPage from './pages/EditPostPage';
import PostDetailPage from './pages/PostDetailPage';

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<Layout />}>
          <Route index element={<HomePage />} />
          <Route path="login" element={<LoginPage />} />
          <Route path="register" element={<RegisterPage />} />
          <Route path="create-post" element={<CreatePostPage />} />
          <Route path="edit-post/:id" element={<EditPostPage />} />
          <Route path="post/:slug" element={<PostDetailPage />} />
          <Route path="*" element={<div className="text-center py-10">Página no encontrada</div>} />
        </Route>
      </Routes>
    </Router>
  );
}

export default App;
