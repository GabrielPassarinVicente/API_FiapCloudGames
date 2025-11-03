import sqlite3

conn = sqlite3.connect('FiapCloudGames.API/FiapCloudGames.db')
cursor = conn.cursor()

cursor.execute("UPDATE Users SET Role = 1 WHERE Email = 'admin@fiap.com'")
conn.commit()

print(f"Linhas atualizadas: {cursor.rowcount}")

cursor.execute("SELECT Id, Name, Email, Role FROM Users WHERE Email = 'admin@fiap.com'")
user = cursor.fetchone()
print(f"Usuário: {user}")

conn.close()
