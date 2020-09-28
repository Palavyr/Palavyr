FROM node:12
WORKDIR /home/ubuntu/PalavyrData
WORKDIR /home/ubuntu/src
COPY package*.json ./
COPY tsconfig.json ./
COPY src/* ./
RUN npm install
RUN npm install -g typescript
RUN npm run build
RUN pwd
COPY . .
EXPOSE 5600
CMD [ "node", "./dist/app.js" ]
