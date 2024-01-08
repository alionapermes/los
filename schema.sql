DROP SCHEMA IF EXISTS "public" CASCADE;
CREATE SCHEMA "public";

CREATE TYPE "GENRE" AS ENUM (
  'mistery', 'fantasy', 'horror', 'romance',
  'thriller', 'memoir', 'adventure', 'graphic novel',
  'western', 'humor', 'history', 'religious',
  'biography', 'science fiction', 'paranormal'
);

CREATE TABLE "book" (
  "id"        SERIAL,
  "title"     TEXT,

  PRIMARY KEY ("id")
);

CREATE TABLE "order" (
  "id"         SERIAL,
  "book_id"    INT NOT NULL,
  "username"   TEXT NOT NULL,
  "ordered_on" DATE NOT NULL,
  "arrives_on" DATE NOT NULL,

  FOREIGN KEY ("book_id") REFERENCES "book"("id")
    ON DELETE SET NULL,

  PRIMARY KEY ("id")
);