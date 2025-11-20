$baseUrl = "http://localhost:5141/api"
$email = "testuser@example.com"
$password = "Password123!"
$nombre = "Test User"

# 1. Authenticate (Register or Login)
Write-Host "Authenticating..."
$token = $null

try {
    $loginBody = @{
        Email = $email
        Password = $password
    } | ConvertTo-Json

    $loginResponse = Invoke-RestMethod -Uri "$baseUrl/auth/login" -Method Post -Body $loginBody -ContentType "application/json" -ErrorAction Stop
    $token = $loginResponse.token
    Write-Host "Logged in successfully."
}
catch {
    Write-Host "Login failed, trying to register..."
    try {
        $registerBody = @{
            NombreUsuario = $nombre
            Email = $email
            Password = $password
        } | ConvertTo-Json

        $registerResponse = Invoke-RestMethod -Uri "$baseUrl/auth/register" -Method Post -Body $registerBody -ContentType "application/json" -ErrorAction Stop
        $token = $registerResponse.token
        Write-Host "Registered and logged in successfully."
    }
    catch {
        Write-Error "Failed to authenticate: $_"
        exit
    }
}

$headers = @{
    Authorization = "Bearer $token"
}

# 2. Create Posts
Write-Host "Creating Posts..."

$posts = @(
    @{
        Titulo = "Bienvenidos a mi Blog Personal"
        Contenido = "Este es el primer post de mi blog. Aquí compartiré mis experiencias aprendiendo desarrollo web con .NET y React. ¡Espero que les guste!"
        Resumen = "Introducción al blog y mis objetivos."
        IdiomaId = 1
        EstadoId = 2 # Publicado
        PermitirComentarios = $true
        CategoriaIds = @()
        EtiquetaIds = @()
    },
    @{
        Titulo = "Guía de C# y .NET 9"
        Contenido = ".NET 9 trae muchas mejoras de rendimiento y nuevas características. En este post exploraremos las novedades en ASP.NET Core y Entity Framework."
        Resumen = "Explorando las novedades de .NET 9."
        IdiomaId = 1
        EstadoId = 2 # Publicado
        PermitirComentarios = $true
        CategoriaIds = @()
        EtiquetaIds = @()
    },
    @{
        Titulo = "Secretos del Desarrollo Frontend"
        Contenido = "Este post es un borrador y no debería ser visible para el público general todavía."
        Resumen = "Borrador sobre frontend."
        IdiomaId = 1
        EstadoId = 1 # Borrador
        PermitirComentarios = $false
        CategoriaIds = @()
        EtiquetaIds = @()
    }
)

$createdPosts = @()

foreach ($post in $posts) {
    try {
        $body = $post | ConvertTo-Json
        $response = Invoke-RestMethod -Uri "$baseUrl/posts" -Method Post -Body $body -Headers $headers -ContentType "application/json"
        $createdPosts += $response
        Write-Host "Created post: $($post.Titulo)"
    }
    catch {
        Write-Error "Failed to create post '$($post.Titulo)': $_"
    }
}

# 3. Create Comments
if ($createdPosts.Count -gt 0) {
    $firstPost = $createdPosts[0]
    Write-Host "Adding comments to post: $($firstPost.titulo)"

    # Comment 1
    $comment1Body = @{
        PostId = $firstPost.id
        Contenido = "¡Excelente iniciativa! Mucha suerte con el blog."
        ComentarioPadreId = $null
    } | ConvertTo-Json

    try {
        $c1 = Invoke-RestMethod -Uri "$baseUrl/comments" -Method Post -Body $comment1Body -Headers $headers -ContentType "application/json"
        Write-Host "Added comment 1"

        # Reply to Comment 1
        $replyBody = @{
            PostId = $firstPost.id
            Contenido = "¡Muchas gracias! Pronto subiré más contenido."
            ComentarioPadreId = $c1.id
        } | ConvertTo-Json

        Invoke-RestMethod -Uri "$baseUrl/comments" -Method Post -Body $replyBody -Headers $headers -ContentType "application/json"
        Write-Host "Added reply to comment 1"
    }
    catch {
        Write-Error "Failed to add comments: $_"
    }

    # Comment 2
    $comment2Body = @{
        PostId = $firstPost.id
        Contenido = "¿Qué tecnologías usaste para construir esto?"
        ComentarioPadreId = $null
    } | ConvertTo-Json

    try {
        Invoke-RestMethod -Uri "$baseUrl/comments" -Method Post -Body $comment2Body -Headers $headers -ContentType "application/json"
        Write-Host "Added comment 2"
    }
    catch {
        Write-Error "Failed to add comment 2: $_"
    }
}

Write-Host "Seeding completed!"
