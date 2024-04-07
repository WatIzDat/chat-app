FROM node:latest 

WORKDIR /app

COPY package.json ./
COPY package-lock.json ./
RUN npm install

COPY . ./

RUN npm run build

EXPOSE 4173
ENV HOST=0.0.0.0

CMD [ "npm", "run", "preview", "--", "--host" ]