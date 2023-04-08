import axios, { AxiosInstance, InternalAxiosRequestConfig } from 'axios';

import appConfig from '../appConfig';
import LoginRequest from './models/Login/LoginRequest';
import LoginResponse from './models/Login/LoginResponse';
import OrdersResponse, { OrdersRequest } from './models/Orders/OrdersResponse';
import ProvidersResponse from './models/Providers/ProvidersResponse';

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
        if(response.status !== 200)
            throw new Error(response.status.toString());
        return response.data;
    }

    public static async RegisterAsync(request: LoginRequest): Promise<LoginResponse> {
        const response = await this.httpClient.post<LoginResponse>("/account/register", request, {validateStatus: _ => true});
        if(response.status !== 200)
            throw new Error(response.status.toString());
        return response.data;
    }

    public static async GetOrdersAsync(request: OrdersRequest): Promise<OrdersResponse> {
        const response = await this.httpClient.post<OrdersResponse>("/orders", request);
        return response.data;
    }

    public static async GetProvidersAsync(): Promise<ProvidersResponse> {
        const response = await this.httpClient.get<ProvidersResponse>("/providers");
        return response.data;
    }
}