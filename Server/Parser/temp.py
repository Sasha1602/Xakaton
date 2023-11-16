from pymongo import MongoClient
from pymongo import ReturnDocument

client = MongoClient('mongodb://localhost:27017/')
db = client.test_database
posts = db.posts # db["posts"]
# post_id = posts.insert_one({"name":"test", "val":123}).inserted_id

db.posts.find_one_and_update({"name": "test"}, {"$set": {"val": 456}}, return_document=ReturnDocument.AFTER)
