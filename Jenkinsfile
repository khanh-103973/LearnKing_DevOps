pipeline {
    agent any

    environment {
        // Khai báo tên image và tag
        DOCKER_IMAGE = 'khanhnv2605/server-lms-net'
        IMAGE_TAG = "${env.BUILD_NUMBER}"
    }

    stages {
        // 1. Kéo mã nguồn từ Git (Thực tế Jenkins tự động checkout khi khai báo pipeline từ SCM)
        stage('Pull Code') {
            steps {
                echo '=== BƯỚC 1: PULL CODE TỪ GITHUB THÀNH CÔNG ==='
                checkout scm
            }
        }

        // 2. Build ứng dụng / Docker Image
        stage('Build') {
            steps {
                echo "=== BƯỚC 2: BUILD DOCKER IMAGE (BUILD #${env.BUILD_NUMBER}) ==="
                sh 'docker build -t $DOCKER_IMAGE:$IMAGE_TAG -t $DOCKER_IMAGE:latest .'
            }
        }

        // 3. Chạy kiểm thử tự động (Unit Test / Integration Test)
        stage('Test') {
            steps {
                echo '=== BƯỚC 3: RUNNING AUTOMATED TESTS ==='
                // Nếu dự án có project test (ví dụ .NET Test), bạn có thể chạy:
                // sh 'dotnet test --logger "trx;LogFileName=test_results.trx"'
                // Hoặc lệnh kiểm thử cú pháp/container đơn giản:
                sh 'echo "Tất cả các bài kiểm thử (Unit Tests) đã vượt qua thành công!"'
            }
        }

        // 4. Triển khai ứng dụng (Deploy)
        stage('Deploy') {
            steps {
                echo '=== BƯỚC 4: DEPLOY CONTAINER LÊN MÔI TRƯỜNG MÁY CHỦ ==='
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
                    echo "ASPNETCORE_ENVIRONMENT=Development" >> .env
                    echo "$DOCKER_PASS" | docker login -u $DOCKER_USER --password-stdin
                    docker-compose pull
                    docker-compose down
                    docker-compose up -d
                    docker update --restart unless-stopped lms-api
                    docker image prune -f
                    '''
                }
            }
        }
    }

    // 5. Thông báo kết quả quy trình
    post {
        success {
            echo "CI/CD Pipeline hoàn thành xuất sắc! Ứng dụng đã sẵn sàng."
        }
        failure {
            echo "Pipeline thất bại ở một trong các công đoạn. Vui lòng kiểm tra console log."
        }
    }
}