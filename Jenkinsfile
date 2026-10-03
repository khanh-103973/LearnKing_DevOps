pipeline {
    agent any
    environment {
        IMAGE_NAME = "server-lms-net"
    }
    stages {
        stage('Checkout') {
            steps {
                checkout([$class: 'GitSCM',
                  branches: [[name: '*/main']],
                  userRemoteConfigs: [[
                    url: 'https://github.com/khanh-103973/LearnKing_DevOps.git',
                    credentialsId: 'github-pat'
                  ]]
                ])
            }
        }
        stage('Docker Build') {
            steps {
                withCredentials([usernamePassword(credentialsId: 'dockerhub-cred',
                    usernameVariable: 'DOCKER_USER', passwordVariable: 'DOCKER_PASS')]) {
                    sh "docker build -t docker.io/$DOCKER_USER/$IMAGE_NAME:latest ."
                }
            }
        }
        stage('Push Docker Hub') {
            steps {
               withCredentials([usernamePassword(credentialsId: 'dockerhub-cred',
                    usernameVariable: 'DOCKER_USER', passwordVariable: 'DOCKER_PASS')]) {
                    sh "echo $DOCKER_PASS | docker login -u $DOCKER_USER --password-stdin"
                    sh "docker push docker.io/$DOCKER_USER/$IMAGE_NAME:latest"
                }
            }
        }
        stage('Deploy Server') {
            steps {
                withCredentials([
                    usernamePassword(credentialsId: 'dockerhub-cred',
                        usernameVariable: 'DOCKER_USER', passwordVariable: 'DOCKER_PASS'),
                    string(credentialsId: 'db-conn', variable: 'DB_CONN'),
                    file(credentialsId: 'docker-compose-file', variable: 'DOCKER_COMPOSE_PATH')
                ]) {
                    sh '''  
                    mkdir -p /var/lib/jenkins/project
                    cp "$DOCKER_COMPOSE_PATH" /var/lib/jenkins/project/docker-compose.yml
                    cd /var/lib/jenkins/project
                    echo "DB_CONNECTION_STRING=$DB_CONN" > .env
                    echo "$DOCKER_PASS" | docker login -u $DOCKER_USER --password-stdin
                    docker compose pull
                    docker compose down
                    docker compose up -d
                    docker image prune -f
                    '''
                }
            }
        }
    }
}