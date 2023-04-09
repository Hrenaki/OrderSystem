import axios, { AxiosInstance, InternalAxiosRequestConfig } from 'axios';

import appConfig from '../appConfig';
import LoginRequest from './models/Login/LoginRequest';
import LoginResponse from './models/Login/LoginResponse';
import OrdersResponse, { OrdersRequest } from './models/Orders/OrdersResponse';
import ProvidersResponse from './models/Providers/ProvidersResponse';
import CreateOrderRequest from './models/Order/CreateOrderRequest';
import CreateOrderResponse from './models/Order/CreateOrderResponse';
import Order from './models/common/Order';
import OrderResponse from './models/Order/OrderResponse';

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

    public static async CreateOrderAsync(request: CreateOrderRequest): Promise<CreateOrderResponse> {
        const response = await this.httpClient.post<CreateOrderResponse>("/order/create", request, {validateStatus: _ => true});
        if(response.status !== 204)
            return {success: false, message: response.statusText};
        return {success: true, message: ''};
    }

    public static async GetOrderAsync(id: number): Promise<Order> {
        const response = await this.httpClient.get<OrderResponse>(`/order/${id}`);
        return response.data.order;
    }
}