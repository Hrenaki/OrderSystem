import axios, { AxiosInstance, AxiosResponse, InternalAxiosRequestConfig } from 'axios';

import appConfig from '../appConfig';
import LoginRequest from './models/LoginRequest';
import LoginResponse from './models/LoginResponse';

const axiosFactory = (): AxiosInstance => {
    const instance = axios.create({
        baseURL: appConfig.API_BASE_URL
    });

    instance.interceptors.request.use((config: InternalAxiosRequestConfig) => {
        const accessToken = localStorage.getItem('access-token');
        if(accessToken) config.headers!.Authorization = `Bearer ${accessToken}`;
        return config;
    }, (error) => Promise.reject(error));

    return instance;
}

export default class OrderSystemAPI {
    private static httpClient: AxiosInstance = axiosFactory();

    public static async LoginAsync(request: LoginRequest): Promise<LoginResponse> {
        const response = await this.httpClient.post<LoginResponse>("/account/login", request, {validateStatus: _ => true});
        if(response.status != 200)
            throw new Error(response.status.toString());
        return response.data;
    }

    public static async RegisterAsync(request: LoginRequest): Promise<LoginResponse> {
        const response = await this.httpClient.post<LoginResponse>("/account/register", request, {validateStatus: _ => true});
        if(response.status != 200)
            throw new Error(response.status.toString());
        return response.data;
    }
}