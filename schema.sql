DROP SCHEMA IF EXISTS "public" CASCADE;
CREATE SCHEMA "public";

CREATE TABLE "genre" (
  "id"    SERIAL,
  "title" TEXT NOT NULL,

  PRIMARY KEY ("id")
);

CREATE TABLE "author" (
  "id"       SERIAL,
  "fullname" TEXT NOT NULL,

  PRIMARY KEY ("id")
);

CREATE TABLE "book" (
  "id"        SERIAL,
  "genre_id"  INT,
  "author_id" INT  NOT NULL,
  "year"      INT  NOT NULL,
  "title"     TEXT NOT NULL,

  FOREIGN KEY ("genre_id") REFERENCES "genre"("id")
    ON DELETE SET NULL,

  FOREIGN KEY ("author_id") REFERENCES "author"("id")
    ON DELETE CASCADE,

  PRIMARY KEY ("id")
);

CREATE TABLE "user" (
  "id"   SERIAL,
  "name" TEXT NOT NULL,

  PRIMARY KEY ("id")
);

CREATE TABLE "order" (
  "id"          SERIAL,
  "user_id"     INT  NOT NULL,
  "book_id"     INT  NOT NULL,
  "secret_code" TEXT NOT NULL,
  "ordered_on"  DATE NOT NULL,
  "arrives_on"  DATE NOT NULL,

  FOREIGN KEY ("user_id") REFERENCES "user"("id")
    ON DELETE CASCADE,

  FOREIGN KEY ("book_id") REFERENCES "book"("id")
    ON DELETE NO ACTION,

  PRIMARY KEY ("id")
);