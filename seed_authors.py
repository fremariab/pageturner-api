import requests
import random

API_URL = "http://localhost:5000/api/v1/authors"  # change if needed

authors = [
    {
        "authorName": "J.K. Rowling",
        "authorBio": "British author best known for the Harry Potter fantasy series."
    },
    {
        "authorName": "George R.R. Martin",
        "authorBio": "American novelist and short story writer, famous for epic fantasy."
    },
    {
        "authorName": "Toni Morrison",
        "authorBio": "Pulitzer and Nobel Prize–winning American novelist."
    },
    {
        "authorName": "Agatha Christie",
        "authorBio": "English writer known for her detective novels and short stories."
    },
    {
        "authorName": "Isaac Asimov",
        "authorBio": "Science fiction writer and professor of authorBiochemistry."
    },
    {
        "authorName": "Chimamanda Ngozi Adichie",
        "authorBio": "Nigerian writer whose works range from novels to short stories."
    },
    {
        "authorName": "Stephen King",
        "authorBio": "American author of horror, supernatural fiction, and fantasy."
    },
    {
        "authorName": "Margaret Atwood",
        "authorBio": "Canadian poet, novelist, and literary critic."
    },
    {
        "authorName": "Haruki Murakami",
        "authorBio": "Japanese writer blending surrealism and realism."
    },
    {
        "authorName": "Octavia E. Butler",
        "authorBio": "Award-winning science fiction author known for speculative themes."
    },
    {
        "authorName": "Neil Gaiman",
        "authorBio": "English author of short fiction, novels, and graphic novels."
    },
    {
        "authorName": "Kazuo Ishiguro",
        "authorBio": "British novelist and Nobel Prize winner."
    },
    {
        "authorName": "Nnedi Okorafor",
        "authorBio": "Nigerian-American author of science fiction and fantasy."
    },
    {
        "authorName": "Brandon Sanderson",
        "authorBio": "Fantasy author known for expansive world-building."
    },
    {
        "authorName": "Ursula K. Le Guin",
        "authorBio": "Influential author of speculative fiction and essays."
    },
    {
        "authorName": "Colson Whitehead",
        "authorBio": "American novelist known for blending historical and speculative fiction."
    },
    {
        "authorName": "James Baldwin",
        "authorBio": "American writer and civil rights activist."
    },
    {
        "authorName": "Sally Rooney",
        "authorBio": "Irish novelist focusing on contemporary relationships."
    },
    {
        "authorName": "Cormac McCarthy",
        "authorBio": "American novelist known for sparse prose and bleak themes."
    },
    {
        "authorName": "Zadie Smith",
        "authorBio": "British novelist and essayist exploring multiculturalism."
    }
]

headers = {
    "Content-Type": "application/json"
}

# Send between 10 and 20 authors
count = random.randint(10, 20)
selected_authors = authors[:count]

for i, author in enumerate(selected_authors, start=1):
    response = requests.post(API_URL, json=author, headers=headers)

    if response.status_code in (200, 201):
        print(f"✅ Author {i} added: {author['authorName']}")
    else:
        print(f"❌ Failed to add {author['authorName']}")
        print(response.status_code, response.text)
