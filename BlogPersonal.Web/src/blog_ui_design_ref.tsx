import { useState } from 'react';
import { Heart, Share2, Search, User, Plus, Menu, X } from 'lucide-react';

interface Post {
  id: number;
  title: string;
  author: string;
  date: string;
  category: string;
  views: number;
  comments: number;
  excerpt: string;
  content: string;
}

export default function BlogUI() {
  const [activeTab, setActiveTab] = useState('home');
  const [mobileMenuOpen, setMobileMenuOpen] = useState(false);
  const [selectedPost, setSelectedPost] = useState<Post | null>(null);

  const mockPosts: Post[] = [
    {
      id: 1,
      title: 'Introducción a .NET Core 9.0',
      author: 'Juan Pérez',
      date: '15 Oct 2025',
      category: 'Tecnología',
      views: 1240,
      comments: 34,
      excerpt: 'Descubre las nuevas características de .NET Core 9.0 y cómo pueden mejorar tus aplicaciones...',
      content: 'Contenido completo del post sobre .NET Core 9.0...'
    },
    {
      id: 2,
      title: 'Entity Framework Core - Mejores Prácticas',
      author: 'María García',
      date: '12 Oct 2025',
      category: 'Programación',
      views: 856,
      comments: 28,
      excerpt: 'Aprende las mejores prácticas para trabajar con Entity Framework Core en tus proyectos...',
      content: 'Contenido sobre mejores prácticas de EF Core...'
    },
    {
      id: 3,
      title: 'Diseño Responsivo con Bootstrap 5',
      author: 'Carlos López',
      date: '10 Oct 2025',
      category: 'Diseño',
      views: 2103,
      comments: 45,
      excerpt: 'Crea interfaces atractivas y responsivas usando Bootstrap 5 en tus aplicaciones web...',
      content: 'Contenido sobre diseño responsivo...'
    }
  ];

  const mockComments = [
    { id: 1, author: 'Usuario 1', date: '5 min', text: 'Excelente artículo, muy bien explicado!' },
    { id: 2, author: 'Usuario 2', date: '1 hora', text: 'Gracias por compartir este conocimiento' }
  ];

  return (
    <div className="min-h-screen bg-gradient-to-br from-slate-900 via-slate-800 to-slate-900">
      {/* Header */}
      <header className="sticky top-0 z-50 bg-slate-950/95 backdrop-blur border-b border-slate-700">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-4 flex items-center justify-between">
          <div className="flex items-center gap-2">
            <div className="w-8 h-8 bg-gradient-to-br from-blue-500 to-purple-600 rounded-lg flex items-center justify-center">
              <span className="text-white font-bold text-lg">B</span>
            </div>
            <h1 className="text-xl font-bold text-white hidden sm:block">Blog Personal</h1>
          </div>

          {/* Desktop Nav */}
          <nav className="hidden md:flex items-center gap-8">
            {['home', 'posts', 'about'].map(item => (
              <button
                key={item}
                onClick={() => setActiveTab(item)}
                className={`capitalize transition-colors ${
                  activeTab === item ? 'text-blue-400' : 'text-slate-400 hover:text-white'
                }`}
              >
                {item}
              </button>
            ))}
          </nav>

          {/* Search & Auth */}
          <div className="flex items-center gap-4">
            <div className="hidden sm:flex bg-slate-800 rounded-lg px-4 py-2 border border-slate-700">
              <Search className="w-4 h-4 text-slate-500" />
              <input
                type="text"
                placeholder="Buscar..."
                className="bg-transparent ml-2 outline-none text-sm text-white placeholder-slate-500 w-48"
              />
            </div>
            <button className="p-2 hover:bg-slate-700 rounded-lg transition-colors" aria-label="Usuario">
              <User className="w-5 h-5 text-slate-400" />
            </button>
            <button
              className="md:hidden p-2"
              onClick={() => setMobileMenuOpen(!mobileMenuOpen)}
              aria-label={mobileMenuOpen ? "Cerrar menú" : "Abrir menú"}
            >
              {mobileMenuOpen ? <X className="w-5 h-5" /> : <Menu className="w-5 h-5" />}
            </button>
          </div>
        </div>
      </header>

      {/* Mobile Menu */}
      {mobileMenuOpen && (
        <div className="md:hidden bg-slate-800 border-b border-slate-700 p-4 space-y-3">
          {['home', 'posts', 'about'].map(item => (
            <button
              key={item}
              onClick={() => {
                setActiveTab(item);
                setMobileMenuOpen(false);
              }}
              className="block w-full text-left capitalize text-slate-400 hover:text-white py-2"
            >
              {item}
            </button>
          ))}
        </div>
      )}

      {/* Main Content */}
      <main className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        {activeTab === 'home' && (
          <>
            {/* Hero Section */}
            <div className="mb-12 bg-gradient-to-r from-blue-600/20 to-purple-600/20 rounded-xl p-8 sm:p-12 border border-blue-500/30">
              <h2 className="text-4xl sm:text-5xl font-bold text-white mb-4">Bienvenido</h2>
              <p className="text-lg text-slate-300 mb-6">
                Explora artículos sobre tecnología, programación y desarrollo web
              </p>
              <button className="bg-blue-600 hover:bg-blue-700 text-white px-6 py-2 rounded-lg transition-colors flex items-center gap-2">
                <Plus className="w-5 h-5" /> Crear Post
              </button>
            </div>

            {/* Posts Grid */}
            <div className="grid md:grid-cols-2 lg:grid-cols-3 gap-6 mb-8">
              {mockPosts.map(post => (
                <div
                  key={post.id}
                  onClick={() => setSelectedPost(post)}
                  className="bg-slate-800 rounded-lg overflow-hidden border border-slate-700 hover:border-blue-500 transition-all hover:shadow-xl hover:shadow-blue-500/20 cursor-pointer group"
                >
                  <div className="h-32 bg-gradient-to-br from-blue-500 to-purple-600 group-hover:scale-105 transition-transform" />
                  <div className="p-6">
                    <div className="flex items-center justify-between mb-3">
                      <span className="text-xs px-3 py-1 bg-blue-500/20 text-blue-400 rounded-full">
                        {post.category}
                      </span>
                      <span className="text-xs text-slate-500">{post.date}</span>
                    </div>
                    <h3 className="text-lg font-semibold text-white mb-2 line-clamp-2">
                      {post.title}
                    </h3>
                    <p className="text-sm text-slate-400 mb-4 line-clamp-2">
                      {post.excerpt}
                    </p>
                    <div className="flex items-center justify-between text-sm text-slate-500 border-t border-slate-700 pt-4">
                      <span className="text-xs">{post.author}</span>
                      <div className="flex gap-3">
                        <span className="flex items-center gap-1">
                          👁️ {post.views}
                        </span>
                        <span className="flex items-center gap-1">
                          💬 {post.comments}
                        </span>
                      </div>
                    </div>
                  </div>
                </div>
              ))}
            </div>
          </>
        )}

        {activeTab === 'posts' && (
          <div className="text-center py-12">
            <h2 className="text-3xl font-bold text-white mb-4">Todos los Posts</h2>
            <p className="text-slate-400">Sección de posts en construcción</p>
          </div>
        )}

        {activeTab === 'about' && (
          <div className="text-center py-12">
            <h2 className="text-3xl font-bold text-white mb-4">Acerca de</h2>
            <p className="text-slate-400">Información del blog en construcción</p>
          </div>
        )}
      </main>

      {/* Post Detail Modal */}
      {selectedPost && (
        <div className="fixed inset-0 bg-black/50 backdrop-blur-sm z-50 flex items-center justify-center p-4">
          <div className="bg-slate-800 rounded-xl max-w-2xl w-full max-h-[90vh] overflow-y-auto border border-slate-700">
            {/* Modal Header */}
            <div className="sticky top-0 bg-slate-900 border-b border-slate-700 p-6 flex items-center justify-between">
              <h2 className="text-2xl font-bold text-white">{selectedPost.title}</h2>
              <button
                onClick={() => setSelectedPost(null)}
                className="text-slate-400 hover:text-white"
                aria-label="Cerrar"
              >
                <X className="w-6 h-6" />
              </button>
            </div>

            {/* Modal Content */}
            <div className="p-6">
              {/* Post Meta */}
              <div className="flex flex-wrap gap-4 mb-6 pb-6 border-b border-slate-700">
                <span className="text-sm text-slate-400">Por <strong className="text-white">{selectedPost.author}</strong></span>
                <span className="text-sm text-slate-400">{selectedPost.date}</span>
                <span className="text-sm text-slate-400">👁️ {selectedPost.views} vistas</span>
              </div>

              {/* Post Content */}
              <div className="mb-8 text-slate-300 leading-relaxed">
                <p className="mb-4">{selectedPost.content}</p>
                <p className="text-slate-400">
                  Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt 
                  ut labore et dolore magna aliqua.
                </p>
              </div>

              {/* Action Buttons */}
              <div className="flex gap-3 mb-8 pb-8 border-b border-slate-700">
                <button className="flex items-center gap-2 text-slate-400 hover:text-blue-400 transition-colors">
                  <Heart className="w-5 h-5" /> Me gusta
                </button>
                <button className="flex items-center gap-2 text-slate-400 hover:text-blue-400 transition-colors">
                  <Share2 className="w-5 h-5" /> Compartir
                </button>
              </div>

              {/* Comments Section */}
              <div>
                <h3 className="text-lg font-semibold text-white mb-4">
                  Comentarios ({mockComments.length})
                </h3>

                {/* Comment Form */}
                <div className="mb-6 p-4 bg-slate-700/30 rounded-lg border border-slate-700">
                  <textarea
                    placeholder="Escribe tu comentario..."
                    className="w-full bg-slate-800 text-white rounded p-3 resize-none focus:outline-none focus:border-blue-500 border border-slate-700 text-sm"
                    rows={3}
                  />
                  <button className="mt-3 bg-blue-600 hover:bg-blue-700 text-white px-4 py-2 rounded transition-colors text-sm">
                    Publicar Comentario
                  </button>
                </div>

                {/* Comments List */}
                <div className="space-y-4">
                  {mockComments.map(comment => (
                    <div key={comment.id} className="bg-slate-700/20 p-4 rounded-lg border border-slate-700">
                      <div className="flex items-center justify-between mb-2">
                        <span className="font-semibold text-white">{comment.author}</span>
                        <span className="text-xs text-slate-500">{comment.date}</span>
                      </div>
                      <p className="text-slate-300 text-sm">{comment.text}</p>
                    </div>
                  ))}
                </div>
              </div>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}