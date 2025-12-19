import json
import requests

# Your API endpoint
API_URL = "http://localhost:5000/api/v1/books"  # Replace with your actual endpoint

# JSON data (paste your full JSON here)
books_json = '''
[
  {
    "bookTitle": "Norwegian Wood",
    "author": "3ea9474c-61e1-4319-8a2f-38f3273b848d",
    "isbn": "978-0375704024",
    "price": 15,
    "stockQuantity": 20,
    "genre": "Fiction"
  },
  {
    "bookTitle": "Kafka on the Shore",
    "author": "3ea9474c-61e1-4319-8a2f-38f3273b848d",
    "isbn": "978-1400079278",
    "price": 18,
    "stockQuantity": 15,
    "genre": "Magical Realism"
  },
  {
    "bookTitle": "1Q84",
    "author": "3ea9474c-61e1-4319-8a2f-38f3273b848d",
    "isbn": "978-0307593313",
    "price": 22,
    "stockQuantity": 10,
    "genre": "Fiction"
  },
  {
    "bookTitle": "The Wind-Up Bird Chronicle",
    "author": "3ea9474c-61e1-4319-8a2f-38f3273b848d",
    "isbn": "978-0679775430",
    "price": 20,
    "stockQuantity": 12,
    "genre": "Fiction"
  },

  {
    "bookTitle": "Murder on the Orient Express",
    "author": "663694e5-03e3-4867-8693-5f6a8bd8600c",
    "isbn": "978-0062693662",
    "price": 12,
    "stockQuantity": 25,
    "genre": "Mystery"
  },
  {
    "bookTitle": "And Then There Were None",
    "author": "663694e5-03e3-4867-8693-5f6a8bd8600c",
    "isbn": "978-0062073488",
    "price": 14,
    "stockQuantity": 30,
    "genre": "Mystery"
  },
  {
    "bookTitle": "The Murder of Roger Ackroyd",
    "author": "663694e5-03e3-4867-8693-5f6a8bd8600c",
    "isbn": "978-0062073563",
    "price": 13,
    "stockQuantity": 18,
    "genre": "Detective"
  },
  {
    "bookTitle": "Death on the Nile",
    "author": "663694e5-03e3-4867-8693-5f6a8bd8600c",
    "isbn": "978-0062073556",
    "price": 15,
    "stockQuantity": 20,
    "genre": "Mystery"
  },

  {
    "bookTitle": "Beloved",
    "author": "77b7d068-80ac-4c01-99e7-8188e12e8b95",
    "isbn": "978-1400033416",
    "price": 16,
    "stockQuantity": 18,
    "genre": "Historical Fiction"
  },
  {
    "bookTitle": "Song of Solomon",
    "author": "77b7d068-80ac-4c01-99e7-8188e12e8b95",
    "isbn": "978-1400033423",
    "price": 17,
    "stockQuantity": 12,
    "genre": "Fiction"
  },
  {
    "bookTitle": "The Bluest Eye",
    "author": "77b7d068-80ac-4c01-99e7-8188e12e8b95",
    "isbn": "978-0307278449",
    "price": 14,
    "stockQuantity": 20,
    "genre": "Fiction"
  },

  {
    "bookTitle": "Harry Potter and the Philosopher's Stone",
    "author": "7b8c4be1-667a-4b58-8cf7-f629ca6367d6",
    "isbn": "978-0747532699",
    "price": 20,
    "stockQuantity": 40,
    "genre": "Fantasy"
  },
  {
    "bookTitle": "Harry Potter and the Chamber of Secrets",
    "author": "7b8c4be1-667a-4b58-8cf7-f629ca6367d6",
    "isbn": "978-0747538493",
    "price": 20,
    "stockQuantity": 35,
    "genre": "Fantasy"
  },
  {
    "bookTitle": "Harry Potter and the Prisoner of Azkaban",
    "author": "7b8c4be1-667a-4b58-8cf7-f629ca6367d6",
    "isbn": "978-0747542155",
    "price": 22,
    "stockQuantity": 30,
    "genre": "Fantasy"
  },
  {
    "bookTitle": "Harry Potter and the Goblet of Fire",
    "author": "7b8c4be1-667a-4b58-8cf7-f629ca6367d6",
    "isbn": "978-0747546245",
    "price": 25,
    "stockQuantity": 28,
    "genre": "Fantasy"
  },

  {
    "bookTitle": "Foundation",
    "author": "80400654-21b7-4f58-8ac0-96803bf12d03",
    "isbn": "978-0553293357",
    "price": 15,
    "stockQuantity": 20,
    "genre": "Science Fiction"
  },
  {
    "bookTitle": "I, Robot",
    "author": "80400654-21b7-4f58-8ac0-96803bf12d03",
    "isbn": "978-0553382563",
    "price": 14,
    "stockQuantity": 25,
    "genre": "Science Fiction"
  },
  {
    "bookTitle": "The Caves of Steel",
    "author": "80400654-21b7-4f58-8ac0-96803bf12d03",
    "isbn": "978-0553293401",
    "price": 16,
    "stockQuantity": 18,
    "genre": "Science Fiction"
  },

  {
    "bookTitle": "American Gods",
    "author": "8a2de2cc-55f4-4764-8843-96581f7667b8",
    "isbn": "978-0062572233",
    "price": 18,
    "stockQuantity": 22,
    "genre": "Fantasy"
  },
  {
    "bookTitle": "Coraline",
    "author": "8a2de2cc-55f4-4764-8843-96581f7667b8",
    "isbn": "978-0380807345",
    "price": 12,
    "stockQuantity": 28,
    "genre": "Fantasy"
  },
  {
    "bookTitle": "Neverwhere",
    "author": "8a2de2cc-55f4-4764-8843-96581f7667b8",
    "isbn": "978-0060557812",
    "price": 15,
    "stockQuantity": 20,
    "genre": "Fantasy"
  },
  {
    "bookTitle": "The Sandman: Preludes & Nocturnes",
    "author": "8a2de2cc-55f4-4764-8843-96581f7667b8",
    "isbn": "978-1401225759",
    "price": 20,
    "stockQuantity": 15,
    "genre": "Graphic Novel"
  },

  {
    "bookTitle": "Purple Hibiscus",
    "author": "a7d06239-feb6-4c25-9fb2-4befaa8a72a8",
    "isbn": "978-1400033423",
    "price": 16,
    "stockQuantity": 18,
    "genre": "Fiction"
  },
  {
    "bookTitle": "Half of a Yellow Sun",
    "author": "a7d06239-feb6-4c25-9fb2-4befaa8a72a8",
    "isbn": "978-1400095209",
    "price": 18,
    "stockQuantity": 22,
    "genre": "Historical Fiction"
  },
  {
    "bookTitle": "Americanah",
    "author": "a7d06239-feb6-4c25-9fb2-4befaa8a72a8",
    "isbn": "978-0307455925",
    "price": 17,
    "stockQuantity": 20,
    "genre": "Fiction"
  },

  {
    "bookTitle": "Kindred",
    "author": "abc4ec79-311c-4a92-a56a-d3916f47d677",
    "isbn": "978-0440218125",
    "price": 15,
    "stockQuantity": 18,
    "genre": "Science Fiction"
  },
  {
    "bookTitle": "Parable of the Sower",
    "author": "abc4ec79-311c-4a92-a56a-d3916f47d677",
    "isbn": "978-0446675505",
    "price": 16,
    "stockQuantity": 20,
    "genre": "Science Fiction"
  },
  {
    "bookTitle": "Dawn",
    "author": "abc4ec79-311c-4a92-a56a-d3916f47d677",
    "isbn": "978-0446675840",
    "price": 14,
    "stockQuantity": 15,
    "genre": "Science Fiction"
  },

  {
    "bookTitle": "A Game of Thrones",
    "author": "b8499330-5cdc-4a31-8674-163534f1ee44",
    "isbn": "978-0553593716",
    "price": 22,
    "stockQuantity": 30,
    "genre": "Fantasy"
  },
  {
    "bookTitle": "A Clash of Kings",
    "author": "b8499330-5cdc-4a31-8674-163534f1ee44",
    "isbn": "978-0553579901",
    "price": 22,
    "stockQuantity": 25,
    "genre": "Fantasy"
  },
  {
    "bookTitle": "A Storm of Swords",
    "author": "b8499330-5cdc-4a31-8674-163534f1ee44",
    "isbn": "978-0553573428",
    "price": 24,
    "stockQuantity": 20,
    "genre": "Fantasy"
  },

  {
    "bookTitle": "The Shining",
    "author": "ba129299-94f6-4d06-bf92-7b77a2e2fec7",
    "isbn": "978-0307743657",
    "price": 15,
    "stockQuantity": 25,
    "genre": "Horror"
  },
  {
    "bookTitle": "It",
    "author": "ba129299-94f6-4d06-bf92-7b77a2e2fec7",
    "isbn": "978-1501142970",
    "price": 20,
    "stockQuantity": 20,
    "genre": "Horror"
  },
  {
    "bookTitle": "Misery",
    "author": "ba129299-94f6-4d06-bf92-7b77a2e2fec7",
    "isbn": "978-1501143106",
    "price": 14,
    "stockQuantity": 22,
    "genre": "Horror"
  },

  {
    "bookTitle": "The Handmaid's Tale",
    "author": "f2d75e49-810b-41fa-9ed8-63429a09bea7",
    "isbn": "978-0385490818",
    "price": 18,
    "stockQuantity": 25,
    "genre": "Dystopian"
  },
  {
    "bookTitle": "Oryx and Crake",
    "author": "f2d75e49-810b-41fa-9ed8-63429a09bea7",
    "isbn": "978-0385721676",
    "price": 17,
    "stockQuantity": 20,
    "genre": "Science Fiction"
  },
  {
    "bookTitle": "The Testaments",
    "author": "f2d75e49-810b-41fa-9ed8-63429a09bea7",
    "isbn": "978-0385543781",
    "price": 20,
    "stockQuantity": 18,
    "genre": "Dystopian"
  }
]

'''

# Load JSON into Python list
books = json.loads(books_json)

# Seed each book into your API
for book in books:
    response = requests.post(API_URL, json=book)
    if response.status_code == 201 or response.status_code == 200:
        print(f"Successfully added: {book['bookTitle']}")
    else:
        print(f"Failed to add: {book['bookTitle']} - Status code: {response.status_code}, Response: {response.text}")
