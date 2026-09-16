import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
    vus: 20,
    duration: '30s',
    insecureSkipTLSVerify: true,
};

export default function () {

    const loginPayload = JSON.stringify({
        email: __ENV.TEST_EMAIL,
        password: __ENV.TEST_PASSWORD,
    });

    const response = http.post(
        'https://localhost:7210/api/Users/login',
        loginPayload,
        {
            headers: {
                'Content-Type': 'application/json',
            },
        }
    );

    check(response, {
        'login status is 200': (r) => r.status === 200,
    });

    sleep(1);
}