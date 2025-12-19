import requests
import json

API_URL = "http://localhost:5000/api/v1/reviews"
reviews = [
  {
    "reviewerName": "Alice Vance",
    "comment": "A masterpiece of misdirection. Agatha Christie at her absolute best.",
    "rating": 5,
    "bookId": "009bd041-4d1b-443e-bd0b-f36566b7b018"
  },
  {
    "reviewerName": "Marcus Thorne",
    "comment": "The suspense is bone-chilling. I couldn't put it down.",
    "rating": 5,
    "bookId": "009bd041-4d1b-443e-bd0b-f36566b7b018"
  },
  {
    "reviewerName": "Sarah Jenks",
    "comment": "Classic mystery. The ending caught me completely off guard.",
    "rating": 4,
    "bookId": "009bd041-4d1b-443e-bd0b-f36566b7b018"
  },
  {
    "reviewerName": "David P.",
    "comment": "A bit dated in parts, but the logic and plot are flawless.",
    "rating": 4,
    "bookId": "009bd041-4d1b-443e-bd0b-f36566b7b018"
  },
  {
    "reviewerName": "Elena Ross",
    "comment": "The gold standard for the 'closed-room' mystery genre.",
    "rating": 5,
    "bookId": "009bd041-4d1b-443e-bd0b-f36566b7b018"
  },
  {
    "reviewerName": "Brian Cho",
    "comment": "The graveyard scene is one of the best moments in the series.",
    "rating": 5,
    "bookId": "0ccb5668-e239-4016-a7fe-c9b2c28fd4cd"
  },
  {
    "reviewerName": "Lily Potter Fan",
    "comment": "The Triwizard Tournament was so well written. Much better than the movie.",
    "rating": 5,
    "bookId": "0ccb5668-e239-4016-a7fe-c9b2c28fd4cd"
  },
  {
    "reviewerName": "Chris Miller",
    "comment": "A bit long, but the world-building is expanding beautifully.",
    "rating": 4,
    "bookId": "0ccb5668-e239-4016-a7fe-c9b2c28fd4cd"
  },
  {
    "reviewerName": "Jordan Smith",
    "comment": "Voldemort's return was genuinely terrifying.",
    "rating": 5,
    "bookId": "0ccb5668-e239-4016-a7fe-c9b2c28fd4cd"
  },
  {
    "reviewerName": "Katie L.",
    "comment": "Loved the Yule Ball drama. A great mix of magic and teenage angst.",
    "rating": 4,
    "bookId": "0ccb5668-e239-4016-a7fe-c9b2c28fd4cd"
  },
  {
    "reviewerName": "Isaac A. Fan",
    "comment": "The original robot detective story. Elijah and Daneel are a great duo.",
    "rating": 5,
    "bookId": "0cf8c459-7f29-4057-8518-f1b6662a4415"
  },
  {
    "reviewerName": "TechGeek99",
    "comment": "Asimov's vision of the future is still so relevant today.",
    "rating": 4,
    "bookId": "0cf8c459-7f29-4057-8518-f1b6662a4415"
  },
  {
    "reviewerName": "Robo Lover",
    "comment": "Interesting social commentary on overpopulation and automation.",
    "rating": 4,
    "bookId": "0cf8c459-7f29-4057-8518-f1b6662a4415"
  },
  {
    "reviewerName": "Miles D.",
    "comment": "A bit slow in the middle, but the world-building is top-notch.",
    "rating": 3,
    "bookId": "0cf8c459-7f29-4057-8518-f1b6662a4415"
  },
  {
    "reviewerName": "Fiona G.",
    "comment": "A brilliant blend of Sci-Fi and Noir mystery.",
    "rating": 5,
    "bookId": "0cf8c459-7f29-4057-8518-f1b6662a4415"
  },
  {
    "reviewerName": "June Osborne",
    "comment": "Devastatingly beautiful and hauntingly prophetic.",
    "rating": 5,
    "bookId": "16490d44-a051-47e2-86cc-925ad88c3bba"
  },
  {
    "reviewerName": "Mark Stevens",
    "comment": "Hard to read at times because of the subject matter, but essential.",
    "rating": 5,
    "bookId": "16490d44-a051-47e2-86cc-925ad88c3bba"
  },
  {
    "reviewerName": "LiteratureLover",
    "comment": "Atwood’s prose is sharp as a knife. A modern classic.",
    "rating": 5,
    "bookId": "16490d44-a051-47e2-86cc-925ad88c3bba"
  },
  {
    "reviewerName": "Emily Watts",
    "comment": "I found the ending a bit abrupt, but the journey was intense.",
    "rating": 4,
    "bookId": "16490d44-a051-47e2-86cc-925ad88c3bba"
  },
  {
    "reviewerName": "Sam R.",
    "comment": "A chilling look at how societies can fracture.",
    "rating": 4,
    "bookId": "16490d44-a051-47e2-86cc-925ad88c3bba"
  },
  {
    "reviewerName": "Toni Fan",
    "comment": "Heartbreaking and profoundly moving. Morrison's voice is unique.",
    "rating": 5,
    "bookId": "1811ed85-7a5f-412f-acf0-99aaa000116c"
  },
  {
    "reviewerName": "Angela D.",
    "comment": "A difficult read due to the themes, but deeply important.",
    "rating": 4,
    "bookId": "1811ed85-7a5f-412f-acf0-99aaa000116c"
  },
  {
    "reviewerName": "Bookworm22",
    "comment": "The prose is poetic and the story is devastating.",
    "rating": 5,
    "bookId": "1811ed85-7a5f-412f-acf0-99aaa000116c"
  },
  {
    "reviewerName": "Reviewer 404",
    "comment": "An exploration of beauty standards that still resonates.",
    "rating": 5,
    "bookId": "1811ed85-7a5f-412f-acf0-99aaa000116c"
  },
  {
    "reviewerName": "Cynthia V.",
    "comment": "Heavy, emotional, and masterfully written.",
    "rating": 4,
    "bookId": "1811ed85-7a5f-412f-acf0-99aaa000116c"
  },
  {
    "reviewerName": "Shadow Moon",
    "comment": "A wild ride through the mythology of America.",
    "rating": 5,
    "bookId": "1acbc33f-1365-44c9-86b2-519363e39e8a"
  },
  {
    "reviewerName": "Gaiman Fan",
    "comment": "Classic Gaiman. Dark, whimsical, and deeply imaginative.",
    "rating": 5,
    "bookId": "1acbc33f-1365-44c9-86b2-519363e39e8a"
  },
  {
    "reviewerName": "Neil S.",
    "comment": "The concept of old gods vs new gods is brilliant.",
    "rating": 4,
    "bookId": "1acbc33f-1365-44c9-86b2-519363e39e8a"
  },
  {
    "reviewerName": "Terry Pratchett fan",
    "comment": "A bit meandering in the middle, but the payoff is worth it.",
    "rating": 4,
    "bookId": "1acbc33f-1365-44c9-86b2-519363e39e8a"
  },
  {
    "reviewerName": "Mythos",
    "comment": "I loved the road trip vibe and the various deities encountered.",
    "rating": 4,
    "bookId": "1acbc33f-1365-44c9-86b2-519363e39e8a"
  },
  {
    "reviewerName": "Hogwarts Alum",
    "comment": "The book that started it all. Pure magic.",
    "rating": 5,
    "bookId": "2508f04f-cf5d-4edd-87a7-a4f2ed805f23"
  },
  {
    "reviewerName": "Ron W.",
    "comment": "I've read this ten times and it never gets old.",
    "rating": 5,
    "bookId": "2508f04f-cf5d-4edd-87a7-a4f2ed805f23"
  },
  {
    "reviewerName": "Hermione G.",
    "comment": "A wonderful introduction to a fantastic world.",
    "rating": 5,
    "bookId": "2508f04f-cf5d-4edd-87a7-a4f2ed805f23"
  },
  {
    "reviewerName": "Dumbledore",
    "comment": "A tale of courage and friendship.",
    "rating": 5,
    "bookId": "2508f04f-cf5d-4edd-87a7-a4f2ed805f23"
  },
  {
    "reviewerName": "Muggle Born",
    "comment": "Even as an adult, this story is enchanting.",
    "rating": 5,
    "bookId": "2508f04f-cf5d-4edd-87a7-a4f2ed805f23"
  },
  {
    "reviewerName": "Murakami Fan",
    "comment": "Melancholic and beautiful. A deeply personal story.",
    "rating": 4,
    "bookId": "265898dc-553f-40e5-b287-f9f1192ff80f"
  },
  {
    "reviewerName": "Naoko's Ghost",
    "comment": "Captured the feeling of the 1960s perfectly.",
    "rating": 5,
    "bookId": "265898dc-553f-40e5-b287-f9f1192ff80f"
  },
  {
    "reviewerName": "Tokyo Reader",
    "comment": "A sad but necessary exploration of loss and growing up.",
    "rating": 4,
    "bookId": "265898dc-553f-40e5-b287-f9f1192ff80f"
  },
  {
    "reviewerName": "Jazz Lover",
    "comment": "The musical references add such a rich layer to the story.",
    "rating": 4,
    "bookId": "265898dc-553f-40e5-b287-f9f1192ff80f"
  },
  {
    "reviewerName": "Kento",
    "comment": "I prefer his more surreal works, but this was still excellent.",
    "rating": 3,
    "bookId": "265898dc-553f-40e5-b287-f9f1192ff80f"
  },
  {
    "reviewerName": "Adichie Stan",
    "comment": "Powerful storytelling about the Biafran War.",
    "rating": 5,
    "bookId": "34f37161-35b4-499f-bf2b-1dcfc65466ae"
  },
  {
    "reviewerName": "Olanna",
    "comment": "The characters feel so real, their pain is palpable.",
    "rating": 5,
    "bookId": "34f37161-35b4-499f-bf2b-1dcfc65466ae"
  },
  {
    "reviewerName": "Kainene",
    "comment": "An epic novel that captures a difficult period in history.",
    "rating": 5,
    "bookId": "34f37161-35b4-499f-bf2b-1dcfc65466ae"
  },
  {
    "reviewerName": "History Buff",
    "comment": "I learned so much about the Nigerian Civil War through these lives.",
    "rating": 4,
    "bookId": "34f37161-35b4-499f-bf2b-1dcfc65466ae"
  },
  {
    "reviewerName": "Ugwu",
    "comment": "The writing is stunning and the story is heart-wrenching.",
    "rating": 5,
    "bookId": "34f37161-35b4-499f-bf2b-1dcfc65466ae"
  },
  {
    "reviewerName": "Hercule Poirot",
    "comment": "A delightful trip down the Nile with a clever mystery.",
    "rating": 4,
    "bookId": "37eaa6e6-cccf-45ac-8fdc-e358f4ed9400"
  },
  {
    "reviewerName": "Mystery Lover",
    "comment": "The setting is exotic and the plot is classic Christie.",
    "rating": 4,
    "bookId": "37eaa6e6-cccf-45ac-8fdc-e358f4ed9400"
  },
  {
    "reviewerName": "Linnet Ridgeway",
    "comment": "The characters are well-drawn and the suspense builds perfectly.",
    "rating": 5,
    "bookId": "37eaa6e6-cccf-45ac-8fdc-e358f4ed9400"
  },
  {
    "reviewerName": "Simon Doyle",
    "comment": "I didn't see that ending coming at all!",
    "rating": 5,
    "bookId": "37eaa6e6-cccf-45ac-8fdc-e358f4ed9400"
  },
  {
    "reviewerName": "Jackie de Bellefort",
    "comment": "A quintessential Poirot adventure. Highly recommended.",
    "rating": 4,
    "bookId": "37eaa6e6-cccf-45ac-8fdc-e358f4ed9400"
  },
  {
    "reviewerName": "Haruki Reader",
    "comment": "An epic, surreal journey. Murakami at his most ambitious.",
    "rating": 5,
    "bookId": "41c0e8c4-57e2-49a2-99e5-cab7424cce83"
  },
  {
    "reviewerName": "Aomame Fan",
    "comment": "The two moons and the parallel worlds are so intriguing.",
    "rating": 4,
    "bookId": "41c0e8c4-57e2-49a2-99e5-cab7424cce83"
  },
  {
    "reviewerName": "Tengo",
    "comment": "A bit long and self-indulgent in places, but still captivating.",
    "rating": 3,
    "bookId": "41c0e8c4-57e2-49a2-99e5-cab7424cce83"
  },
  {
    "reviewerName": "Little People",
    "comment": "I loved the blend of mystery, romance, and the supernatural.",
    "rating": 5,
    "bookId": "41c0e8c4-57e2-49a2-99e5-cab7424cce83"
  },
  {
    "reviewerName": "Murakami Obsessed",
    "comment": "A masterpiece that stays with you long after finishing.",
    "rating": 5,
    "bookId": "41c0e8c4-57e2-49a2-99e5-cab7424cce83"
  },
  {
    "reviewerName": "Westeros Resident",
    "comment": "The politics and intrigue are even more intense in this one.",
    "rating": 5,
    "bookId": "46979223-dc11-4e74-b208-1a18e61669a3"
  },
  {
    "reviewerName": "Tyrion Fan",
    "comment": "The Battle of the Blackwater was epic! Martin is a master.",
    "rating": 5,
    "bookId": "46979223-dc11-4e74-b208-1a18e61669a3"
  },
  {
    "reviewerName": "Arya Stark",
    "comment": "Loved the character development, especially for the younger characters.",
    "rating": 4,
    "bookId": "46979223-dc11-4e74-b208-1a18e61669a3"
  },
  {
    "reviewerName": "Joffrey Hater",
    "comment": "A fantastic sequel that lives up to the hype.",
    "rating": 4,
    "bookId": "46979223-dc11-4e74-b208-1a18e61669a3"
  },
  {
    "reviewerName": "Winter Is Coming",
    "comment": "The stakes are raised and the world keeps expanding.",
    "rating": 5,
    "bookId": "46979223-dc11-4e74-b208-1a18e61669a3"
  },
  {
    "reviewerName": "Red Wedding Survivor",
    "comment": "This book broke me. In the best/worst possible way.",
    "rating": 5,
    "bookId": "4b58f0d1-8fab-44e4-a2cc-758e89cc3992"
  },
  {
    "reviewerName": "Jon Snow",
    "comment": "The best book in the series so far. Action-packed and emotional.",
    "rating": 5,
    "bookId": "4b58f0d1-8fab-44e4-a2cc-758e89cc3992"
  },
  {
    "reviewerName": "Daenerys",
    "comment": "So many twists and turns! I couldn't stop reading.",
    "rating": 5,
    "bookId": "4b58f0d1-8fab-44e4-a2cc-758e89cc3992"
  },
  {
    "reviewerName": "Oberyn Martell",
    "comment": "Martin really knows how to keep you on the edge of your seat.",
    "rating": 4,
    "bookId": "4b58f0d1-8fab-44e4-a2cc-758e89cc3992"
  },
  {
    "reviewerName": "Lannister Always Pays",
    "comment": "A monumental achievement in fantasy literature.",
    "rating": 5,
    "bookId": "4b58f0d1-8fab-44e4-a2cc-758e89cc3992"
  },
  {
    "reviewerName": "Poirot Fan",
    "comment": "The most famous train mystery for a reason. Brilliant.",
    "rating": 5,
    "bookId": "5351142a-fa48-4b2b-8c9c-215a96bf56e4"
  },
  {
    "reviewerName": "Train Traveler",
    "comment": "The solution is legendary. A must-read for mystery fans.",
    "rating": 5,
    "bookId": "5351142a-fa48-4b2b-8c9c-215a96bf56e4"
  },
  {
    "reviewerName": "Ratchett",
    "comment": "Christie's character work is exceptional in this confined setting.",
    "rating": 4,
    "bookId": "5351142a-fa48-4b2b-8c9c-215a96bf56e4"
  },
  {
    "reviewerName": "Mystery Solver",
    "comment": "A quick, engaging, and incredibly clever read.",
    "rating": 4,
    "bookId": "5351142a-fa48-4b2b-8c9c-215a96bf56e4"
  },
  {
    "reviewerName": "Snowbound",
    "comment": "Classic Christie at the top of her game.",
    "rating": 5,
    "bookId": "5351142a-fa48-4b2b-8c9c-215a96bf56e4"
  },
  {
    "reviewerName": "Hari Seldon",
    "comment": "The granddaddy of space operas. Psychohistory is a fascinating concept.",
    "rating": 5,
    "bookId": "5cac14f5-1227-4c09-9b30-32ce49ec3c61"
  },
  {
    "reviewerName": "Sci-Fi Geek",
    "comment": "Asimov's scale is incredible. This covers centuries in just one book.",
    "rating": 4,
    "bookId": "5cac14f5-1227-4c09-9b30-32ce49ec3c61"
  },
  {
    "reviewerName": "Terminus Resident",
    "comment": "A bit dry and academic at times, but the ideas are revolutionary.",
    "rating": 3,
    "bookId": "5cac14f5-1227-4c09-9b30-32ce49ec3c61"
  },
  {
    "reviewerName": "Galactic Empire",
    "comment": "Essential reading for any science fiction fan.",
    "rating": 4,
    "bookId": "5cac14f5-1227-4c09-9b30-32ce49ec3c61"
  },
  {
    "reviewerName": "The Mule",
    "comment": "The scope of this story is just mind-blowing.",
    "rating": 5,
    "bookId": "5cac14f5-1227-4c09-9b30-32ce49ec3c61"
  },
  {
    "reviewerName": "Toru Okada",
    "comment": "A labyrinthine journey through identity and the subconscious.",
    "rating": 5,
    "bookId": "660bcf1c-39e5-4894-8fa3-a916bc3999fb"
  },
  {
    "reviewerName": "The Cat",
    "comment": "So many strange and wonderful characters. Typical Murakami.",
    "rating": 4,
    "bookId": "660bcf1c-39e5-4894-8fa3-a916bc3999fb"
  },
  {
    "reviewerName": "Well Dweller",
    "comment": "This book is like a dream you don't want to wake up from.",
    "rating": 5,
    "bookId": "660bcf1c-39e5-4894-8fa3-a916bc3999fb"
  },
  {
    "reviewerName": "May Kasahara",
    "comment": "A bit long and confusing, but the atmosphere is incredible.",
    "rating": 4,
    "bookId": "660bcf1c-39e5-4894-8fa3-a916bc3999fb"
  },
  {
    "reviewerName": "Bird Fan",
    "comment": "One of my favorite Murakami novels. Truly surreal and moving.",
    "rating": 5,
    "bookId": "660bcf1c-39e5-4894-8fa3-a916bc3999fb"
  },
  {
    "reviewerName": "Kambili",
    "comment": "A beautiful and tragic coming-of-age story.",
    "rating": 5,
    "bookId": "6971c199-298f-4428-82f1-6c0561219f3d"
  },
  {
    "reviewerName": "Jaja",
    "comment": "Adichie's writing is so evocative and powerful.",
    "rating": 4,
    "bookId": "6971c199-298f-4428-82f1-6c0561219f3d"
  },
  {
    "reviewerName": "Aunty Ifeoma",
    "comment": "A nuanced exploration of family, religion, and freedom.",
    "rating": 5,
    "bookId": "6971c199-298f-4428-82f1-6c0561219f3d"
  },
  {
    "reviewerName": "Papa's Daughter",
    "comment": "Heartbreaking but ultimately hopeful.",
    "rating": 4,
    "bookId": "6971c199-298f-4428-82f1-6c0561219f3d"
  },
  {
    "reviewerName": "Nigeria Reader",
    "comment": "A stunning debut novel. Adichie is a gifted storyteller.",
    "rating": 5,
    "bookId": "6971c199-298f-4428-82f1-6c0561219f3d"
  },
  {
    "reviewerName": "Roger Ackroyd",
    "comment": "The most shocking twist in detective fiction. Ever.",
    "rating": 5,
    "bookId": "7426b000-f71d-46a0-b862-7c9a152540ca"
  },
  {
    "reviewerName": "Poirot Fanatic",
    "comment": "I never saw it coming. Christie is the queen of mystery.",
    "rating": 5,
    "bookId": "7426b000-f71d-46a0-b862-7c9a152540ca"
  },
  {
    "reviewerName": "Dr. Sheppard",
    "comment": "A masterclass in unreliable narration.",
    "rating": 5,
    "bookId": "7426b000-f71d-46a0-b862-7c9a152540ca"
  },
  {
    "reviewerName": "Mystery Addict",
    "comment": "The logic is perfect and the execution is flawless.",
    "rating": 4,
    "bookId": "7426b000-f71d-46a0-b862-7c9a152540ca"
  },
  {
    "reviewerName": "Flora Ackroyd",
    "comment": "One of Christie's best works. Absolutely brilliant.",
    "rating": 5,
    "bookId": "7426b000-f71d-46a0-b862-7c9a152540ca"
  },
  {
    "reviewerName": "Ned Stark Fan",
    "comment": "A brutal and brilliant introduction to Westeros.",
    "rating": 5,
    "bookId": "7ab69a85-dd29-4cf1-aafb-7235f0e99d71"
  },
  {
    "reviewerName": "George R.R. Martin",
    "comment": "I'm still traumatized by the ending, but I love it.",
    "rating": 5,
    "bookId": "7ab69a85-dd29-4cf1-aafb-7235f0e99d71"
  },
  {
    "reviewerName": "Tyrion Lannister",
    "comment": "The world-building is unparalleled in modern fantasy.",
    "rating": 5,
    "bookId": "7ab69a85-dd29-4cf1-aafb-7235f0e99d71"
  },
  {
    "reviewerName": "Arya Fan",
    "comment": "A game-changer for the fantasy genre.",
    "rating": 4,
    "bookId": "7ab69a85-dd29-4cf1-aafb-7235f0e99d71"
  },
  {
    "reviewerName": "Winter Is Coming",
    "comment": "I couldn't put it down. Can't wait for the next one.",
    "rating": 5,
    "bookId": "7ab69a85-dd29-4cf1-aafb-7235f0e99d71"
  },
  {
    "reviewerName": "Sirius Black",
    "comment": "The best book in the series. The plot is so clever.",
    "rating": 5,
    "bookId": "7b1db656-ec35-4750-ae00-367ee087fdc7"
  },
  {
    "reviewerName": "Buckbeak",
    "comment": "The introduction of Sirius and Remus was fantastic.",
    "rating": 5,
    "bookId": "7b1db656-ec35-4750-ae00-367ee087fdc7"
  },
  {
    "reviewerName": "Marauder Fan",
    "comment": "I loved the time-turner sequence! So well executed.",
    "rating": 5,
    "bookId": "7b1db656-ec35-4750-ae00-367ee087fdc7"
  },
  {
    "reviewerName": "Remus Lupin",
    "comment": "A slightly darker tone, but it suits the growing characters.",
    "rating": 5,
    "bookId": "7b1db656-ec35-4750-ae00-367ee087fdc7"
  },
  {
    "reviewerName": "Azbakan Escapee",
    "comment": "Simply magical. My personal favorite Harry Potter book.",
    "rating": 5,
    "bookId": "7b1db656-ec35-4750-ae00-367ee087fdc7"
  },
  {
    "reviewerName": "Lilith Iyapo",
    "comment": "Octavia Butler's imagination is incredible. Aliens that are truly alien.",
    "rating": 5,
    "bookId": "7d92d237-92a7-43e0-84b0-ec1ade5c4ad8"
  },
  {
    "reviewerName": "Oankali Fan",
    "comment": "A thought-provoking exploration of what it means to be human.",
    "rating": 5,
    "bookId": "7d92d237-92a7-43e0-84b0-ec1ade5c4ad8"
  },
  {
    "reviewerName": "Sci-Fi Fan",
    "comment": "The biology and sociology of the Oankali are fascinating.",
    "rating": 4,
    "bookId": "7d92d237-92a7-43e0-84b0-ec1ade5c4ad8"
  },
  {
    "reviewerName": "Earth Survivor",
    "comment": "A challenging but rewarding read. Butler is a visionary.",
    "rating": 4,
    "bookId": "7d92d237-92a7-43e0-84b0-ec1ade5c4ad8"
  },
  {
    "reviewerName": "Xenogenesis",
    "comment": "A brilliant start to a unique and compelling trilogy.",
    "rating": 5,
    "bookId": "7d92d237-92a7-43e0-84b0-ec1ade5c4ad8"
  },
  {
    "reviewerName": "Dana James",
    "comment": "A harrowing and essential exploration of slavery and history.",
    "rating": 5,
    "bookId": "7fe76444-d71c-48aa-8e70-106f159a009d"
  },
  {
    "reviewerName": "Rufus Fan",
    "comment": "The time travel element makes the horrors of the past feel so immediate.",
    "rating": 5,
    "bookId": "7fe76444-d71c-48aa-8e70-106f159a009d"
  },
  {
    "reviewerName": "Historical Fiction Buff",
    "comment": "Butler's writing is sparse and powerful. A must-read.",
    "rating": 5,
    "bookId": "7fe76444-d71c-48aa-8e70-106f159a009d"
  },
  {
    "reviewerName": "Maryland Resident",
    "comment": "A devastatingly honest look at power and survival.",
    "rating": 4,
    "bookId": "7fe76444-d71c-48aa-8e70-106f159a009d"
  },
  {
    "reviewerName": "Butler Devotee",
    "comment": "One of the most impactful books I've ever read.",
    "rating": 5,
    "bookId": "7fe76444-d71c-48aa-8e70-106f159a009d"
  },
  {
    "reviewerName": "Snowman",
    "comment": "A terrifyingly plausible look at a corporate-run dystopia.",
    "rating": 4,
    "bookId": "875cd209-7c53-4996-bf0d-149a72e93f07"
  },
  {
    "reviewerName": "Crake Fan",
    "comment": "Atwood's world-building is as sharp and cynical as ever.",
    "rating": 5,
    "bookId": "875cd209-7c53-4996-bf0d-149a72e93f07"
  },
  {
    "reviewerName": "Oryx Fan",
    "comment": "The pigoons and other bio-engineered creatures are nightmare fuel.",
    "rating": 4,
    "bookId": "875cd209-7c53-4996-bf0d-149a72e93f07"
  },
  {
    "reviewerName": "Dystopian Fan",
    "comment": "A brilliant and disturbing look at the dangers of unchecked science.",
    "rating": 4,
    "bookId": "875cd209-7c53-4996-bf0d-149a72e93f07"
  },
  {
    "reviewerName": "MaddAddam",
    "comment": "One of Atwood's best works. Gripping and thought-provoking.",
    "rating": 5,
    "bookId": "875cd209-7c53-4996-bf0d-149a72e93f07"
  },
  {
    "reviewerName": "Lauren Olamina",
    "comment": "A prophetic and chillingly relevant look at social collapse.",
    "rating": 5,
    "bookId": "9225ae57-13d1-4332-813b-fd272acd8622"
  },
  {
    "reviewerName": "Earthseed Follower",
    "comment": "Butler's vision of the future is both terrifying and hopeful.",
    "rating": 5,
    "bookId": "9225ae57-13d1-4332-813b-fd272acd8622"
  },
  {
    "reviewerName": "Acorn Resident",
    "comment": "The philosophy of Earthseed is so interesting and well-developed.",
    "rating": 5,
    "bookId": "9225ae57-13d1-4332-813b-fd272acd8622"
  },
  {
    "reviewerName": "Dystopian Junkie",
    "comment": "A powerful story about survival, community, and change.",
    "rating": 4,
    "bookId": "9225ae57-13d1-4332-813b-fd272acd8622"
  },
  {
    "reviewerName": "Octavia's Disciple",
    "comment": "An essential read for understanding our own world.",
    "rating": 5,
    "bookId": "9225ae57-13d1-4332-813b-fd272acd8622"
  },
  {
    "reviewerName": "Milkman Dead",
    "comment": "A lyrical and epic exploration of family history and flight.",
    "rating": 5,
    "bookId": "9baeb76e-3b82-4b37-bd96-ace9b3e57f9d"
  },
  {
    "reviewerName": "Pilate Fan",
    "comment": "Morrison's prose is just magical. A truly great American novel.",
    "rating": 5,
    "bookId": "9baeb76e-3b82-4b37-bd96-ace9b3e57f9d"
  },
  {
    "reviewerName": "Michigan Reader",
    "comment": "A complex and rewarding story that rewards multiple readings.",
    "rating": 4,
    "bookId": "9baeb76e-3b82-4b37-bd96-ace9b3e57f9d"
  },
  {
    "reviewerName": "Literature Student",
    "comment": "The mythology and symbolism in this book are so rich.",
    "rating": 5,
    "bookId": "9baeb76e-3b82-4b37-bd96-ace9b3e57f9d"
  },
  {
    "reviewerName": "Toni Morrison Fan",
    "comment": "A masterpiece of African American literature. Simply beautiful.",
    "rating": 5,
    "bookId": "9baeb76e-3b82-4b37-bd96-ace9b3e57f9d"
  },
  {
    "reviewerName": "Sethe Fan",
    "comment": "A haunting and powerful exploration of motherhood and slavery.",
    "rating": 5,
    "bookId": "a2553781-cbc4-4126-b056-114f71cd93e0"
  },
  {
    "reviewerName": "Denver",
    "comment": "One of the most intense and emotional books I've ever read.",
    "rating": 5,
    "bookId": "a2553781-cbc4-4126-b056-114f71cd93e0"
  },
  {
    "reviewerName": "Paul D.",
    "comment": "Morrison's language is breathtaking and the story is devastating.",
    "rating": 5,
    "bookId": "a2553781-cbc4-4126-b056-114f71cd93e0"
  },
  {
    "reviewerName": "Beloved's Ghost",
    "comment": "A masterpiece of historical fiction and magical realism.",
    "rating": 5,
    "bookId": "a2553781-cbc4-4126-b056-114f71cd93e0"
  },
  {
    "reviewerName": "Literary Critic",
    "comment": "A profound meditation on the lasting trauma of slavery.",
    "rating": 5,
    "bookId": "a2553781-cbc4-4126-b056-114f71cd93e0"
  },
  {
    "reviewerName": "Dream Fan",
    "comment": "The perfect introduction to Morpheus and the Dreaming.",
    "rating": 5,
    "bookId": "a93ea22f-36e0-4258-a4f8-95bf786af018"
  },
  {
    "reviewerName": "Comic Book Geek",
    "comment": "Gaiman's imagination is just incredible. The artwork is great too.",
    "rating": 4,
    "bookId": "a93ea22f-36e0-4258-a4f8-95bf786af018"
  },
  {
    "reviewerName": "Morpheus Follower",
    "comment": "A unique and compelling blend of mythology and horror.",
    "rating": 5,
    "bookId": "a93ea22f-36e0-4258-a4f8-95bf786af018"
  },
  {
    "reviewerName": "Sandman Fan",
    "comment": "The stories are so varied and imaginative. I'm hooked.",
    "rating": 4,
    "bookId": "a93ea22f-36e0-4258-a4f8-95bf786af018"
  },
  {
    "reviewerName": "Graphic Novel Fan",
    "comment": "One of the best graphic novel series ever written.",
    "rating": 5,
    "bookId": "a93ea22f-36e0-4258-a4f8-95bf786af018"
  },
  {
    "reviewerName": "Kafka Tamura",
    "comment": "A surreal and beautiful journey through time and memory.",
    "rating": 5,
    "bookId": "ab4aa673-1ba9-4255-a527-de99dadeaaa5"
  },
  {
    "reviewerName": "Nakata Fan",
    "comment": "Murakami at his best. So many strange and wonderful things.",
    "rating": 5,
    "bookId": "ab4aa673-1ba9-4255-a527-de99dadeaaa5"
  },
  {
    "reviewerName": "Talking Cat",
    "comment": "A bit confusing at times, but the atmosphere is so captivating.",
    "rating": 4,
    "bookId": "ab4aa673-1ba9-4255-a527-de99dadeaaa5"
  },
  {
    "reviewerName": "Library Resident",
    "comment": "A unique and dreamlike story that stays with you long after.",
    "rating": 4,
    "bookId": "ab4aa673-1ba9-4255-a527-de99dadeaaa5"
  },
  {
    "reviewerName": "Murakami Obsessive",
    "comment": "One of Murakami's most imaginative and rewarding novels.",
    "rating": 5,
    "bookId": "ab4aa673-1ba9-4255-a527-de99dadeaaa5"
  },
  {
    "reviewerName": "Paul Sheldon Fan",
    "comment": "A terrifyingly intimate look at obsession and isolation.",
    "rating": 5,
    "bookId": "ae0c1039-afd1-40ef-bb7f-36f22ae16a85"
  },
  {
    "reviewerName": "Annie Wilkes",
    "comment": "King at his most suspenseful. I couldn't put it down.",
    "rating": 5,
    "bookId": "ae0c1039-afd1-40ef-bb7f-36f22ae16a85"
  },
  {
    "reviewerName": "Horror Junkie",
    "comment": "A masterclass in psychological horror. So gripping.",
    "rating": 5,
    "bookId": "ae0c1039-afd1-40ef-bb7f-36f22ae16a85"
  },
  {
    "reviewerName": "Stephen King Fan",
    "comment": "One of King's best and most focused novels.",
    "rating": 4,
    "bookId": "ae0c1039-afd1-40ef-bb7f-36f22ae16a85"
  },
  {
    "reviewerName": "Number One Fan",
    "comment": "A truly terrifying story about the dark side of fandom.",
    "rating": 5,
    "bookId": "ae0c1039-afd1-40ef-bb7f-36f22ae16a85"
  },
  {
    "reviewerName": "Coraline Jones",
    "comment": "A deliciously dark and whimsical modern fairy tale.",
    "rating": 5,
    "bookId": "b01dd014-eacd-41a4-afec-1454ce4e6777"
  },
  {
    "reviewerName": "Other Mother Fan",
    "comment": "Gaiman's imagination is just wonderful. Truly unique.",
    "rating": 5,
    "bookId": "b01dd014-eacd-41a4-afec-1454ce4e6777"
  },
  {
    "reviewerName": "Button Eye",
    "comment": "A perfect blend of wonder and dread for all ages.",
    "rating": 5,
    "bookId": "b01dd014-eacd-41a4-afec-1454ce4e6777"
  },
  {
    "reviewerName": "Cat Fan",
    "comment": "A short but incredibly impactful and memorable story.",
    "rating": 4,
    "bookId": "b01dd014-eacd-41a4-afec-1454ce4e6777"
  },
  {
    "reviewerName": "Gaiman Devotee",
    "comment": "One of Gaiman's best. A true classic of children's literature.",
    "rating": 5,
    "bookId": "b01dd014-eacd-41a4-afec-1454ce4e6777"
  },
  {
    "reviewerName": "Dobby Fan",
    "comment": "A great sequel that builds on the world of the first book.",
    "rating": 4,
    "bookId": "b034a368-d314-41c4-a4af-c959910fbee0"
  },
  {
    "reviewerName": "Basilisk Hunter",
    "comment": "The mystery of the Chamber was so well-developed.",
    "rating": 4,
    "bookId": "b034a368-d314-41c4-a4af-c959910fbee0"
  },
  {
    "reviewerName": "Tom Riddle Fan",
    "comment": "A bit scarier than the first one, but still so much fun.",
    "rating": 5,
    "bookId": "b034a368-d314-41c4-a4af-c959910fbee0"
  },
  {
    "reviewerName": "Ginny Weasley Fan",
    "comment": "I loved the flying car sequence! So imaginative.",
    "rating": 4,
    "bookId": "b034a368-d314-41c4-a4af-c959910fbee0"
  },
  {
    "reviewerName": "Hogwarts Student",
    "comment": "Another magical entry in a truly fantastic series.",
    "rating": 5,
    "bookId": "b034a368-d314-41c4-a4af-c959910fbee0"
  },
  {
    "reviewerName": "Pennywise Fan",
    "comment": "An epic and terrifying look at childhood and trauma.",
    "rating": 5,
    "bookId": "bb3dc01b-2fbd-4b95-9480-d8ab5aefbd55"
  },
  {
    "reviewerName": "Losers Club Member",
    "comment": "King's best work. A monumental achievement in horror.",
    "rating": 5,
    "bookId": "bb3dc01b-2fbd-4b95-9480-d8ab5aefbd55"
  },
  {
    "reviewerName": "Derry Resident",
    "comment": "A bit long and meandering at times, but so powerful.",
    "rating": 4,
    "bookId": "bb3dc01b-2fbd-4b95-9480-d8ab5aefbd55"
  },
  {
    "reviewerName": "Horror Fanatic",
    "comment": "One of the most terrifying books I've ever read.",
    "rating": 5,
    "bookId": "bb3dc01b-2fbd-4b95-9480-d8ab5aefbd55"
  },
  {
    "reviewerName": "Stephen King Junkie",
    "comment": "A masterpiece of storytelling. Simply incredible.",
    "rating": 5,
    "bookId": "bb3dc01b-2fbd-4b95-9480-d8ab5aefbd55"
  },
  {
    "reviewerName": "Susan Calvin Fan",
    "comment": "The foundational book for modern robot science fiction.",
    "rating": 5,
    "bookId": "c7535f02-d516-494c-ad36-34b1804aafff"
  },
  {
    "reviewerName": "Three Laws Follower",
    "comment": "Asimov's logic and puzzle-solving stories are so clever.",
    "rating": 4,
    "bookId": "c7535f02-d516-494c-ad36-34b1804aafff"
  },
  {
    "reviewerName": "Robot Lover",
    "comment": "A series of fascinating and thought-provoking stories.",
    "rating": 5,
    "bookId": "c7535f02-d516-494c-ad36-34b1804aafff"
  },
  {
    "reviewerName": "Sci-Fi Geek",
    "comment": "Essential reading for anyone interested in AI and robotics.",
    "rating": 4,
    "bookId": "c7535f02-d516-494c-ad36-34b1804aafff"
  },
  {
    "reviewerName": "Asimov Devotee",
    "comment": "One of the most influential science fiction books ever written.",
    "rating": 5,
    "bookId": "c7535f02-d516-494c-ad36-34b1804aafff"
  },
  {
    "reviewerName": "Agnes Fan",
    "comment": "A worthy sequel that expands the world of Gilead.",
    "rating": 4,
    "bookId": "d51e3f80-94ab-40d7-b280-bec3a9e07369"
  },
  {
    "reviewerName": "Nicole Fan",
    "comment": "Atwood's writing is as sharp and powerful as ever.",
    "rating": 5,
    "bookId": "d51e3f80-94ab-40d7-b280-bec3a9e07369"
  },
  {
    "reviewerName": "Lydia Fan",
    "comment": "A more hopeful story than the first one, but still powerful.",
    "rating": 4,
    "bookId": "d51e3f80-94ab-40d7-b280-bec3a9e07369"
  },
  {
    "reviewerName": "Handmaid's Tale Fan",
    "comment": "A gripping and satisfying conclusion to the story.",
    "rating": 5,
    "bookId": "d51e3f80-94ab-40d7-b280-bec3a9e07369"
  },
  {
    "reviewerName": "Atwood Devotee",
    "comment": "Another masterpiece from a truly great writer.",
    "rating": 5,
    "bookId": "d51e3f80-94ab-40d7-b280-bec3a9e07369"
  },
  {
    "reviewerName": "Ifemelu Fan",
    "comment": "A brilliant and witty look at race, identity, and migration.",
    "rating": 5,
    "bookId": "e41e34b2-7e30-468a-91ed-412bc8fe2fdb"
  },
  {
    "reviewerName": "Obinze Fan",
    "comment": "Adichie's voice is so authentic and engaging.",
    "rating": 5,
    "bookId": "e41e34b2-7e30-468a-91ed-412bc8fe2fdb"
  },
  {
    "reviewerName": "Nigerian Reader",
    "comment": "A powerful and insightful novel about the immigrant experience.",
    "rating": 5,
    "bookId": "e41e34b2-7e30-468a-91ed-412bc8fe2fdb"
  },
  {
    "reviewerName": "Americanah Fan",
    "comment": "A sprawling and ambitious novel that hits all the right notes.",
    "rating": 4,
    "bookId": "e41e34b2-7e30-468a-91ed-412bc8fe2fdb"
  },
  {
    "reviewerName": "Adichie Devotee",
    "comment": "One of the best novels of the decade. Simply fantastic.",
    "rating": 5,
    "bookId": "e41e34b2-7e30-468a-91ed-412bc8fe2fdb"
  },
  {
    "reviewerName": "Richard Mayhew",
    "comment": "A wonderful urban fantasy set in the London Below.",
    "rating": 5,
    "bookId": "e5b1ad40-b25b-4173-b1ae-3b40eb8fb8f2"
  },
  {
    "reviewerName": "Door Fan",
    "comment": "Gaiman's imagination is just incredible. Such a fun world.",
    "rating": 5,
    "bookId": "e5b1ad40-b25b-4173-b1ae-3b40eb8fb8f2"
  },
  {
    "reviewerName": "Marquis de Carabas",
    "comment": "A unique and captivating story with great characters.",
    "rating": 4,
    "bookId": "e5b1ad40-b25b-4173-b1ae-3b40eb8fb8f2"
  },
  {
    "reviewerName": "Neverwhere Fan",
    "comment": "A bit dark and whimsical, just the way I like it.",
    "rating": 4,
    "bookId": "e5b1ad40-b25b-4173-b1ae-3b40eb8fb8f2"
  },
  {
    "reviewerName": "Gaiman Obsessive",
    "comment": "One of my favorite Gaiman books. Truly magical.",
    "rating": 5,
    "bookId": "e5b1ad40-b25b-4173-b1ae-3b40eb8fb8f2"
  },
  {
    "reviewerName": "Jack Torrance Fan",
    "comment": "A terrifyingly plausible look at alcoholism and isolation.",
    "rating": 5,
    "bookId": "f9911386-62f7-4e27-9fd2-193e39192c9f"
  },
  {
    "reviewerName": "Danny Torrance",
    "comment": "King's best work. A masterpiece of psychological horror.",
    "rating": 5,
    "bookId": "f9911386-62f7-4e27-9fd2-193e39192c9f"
  },
  {
    "reviewerName": "Overlook Hotel Resident",
    "comment": "A truly terrifying story about a haunted house.",
    "rating": 5,
    "bookId": "f9911386-62f7-4e27-9fd2-193e39192c9f"
  },
  {
    "reviewerName": "Horror Junkie",
    "comment": "One of the most frightening books I've ever read.",
    "rating": 4,
    "bookId": "f9911386-62f7-4e27-9fd2-193e39192c9f"
  },
  {
    "reviewerName": "Stephen King Fanatic",
    "comment": "A classic of modern horror literature. Simply incredible.",
    "rating": 5,
    "bookId": "f9911386-62f7-4e27-9fd2-193e39192c9f"
  }
]
headers = {"Content-Type": "application/json"}

for i, review in enumerate(reviews, start=1):
    response = requests.post(API_URL, json=review, headers=headers)

    if response.status_code in (200, 201):
        print(f"✅ Review {i} sent successfully")
    else:
        print(f"❌ Review {i} failed: {response.status_code}")
        print(response.text)
