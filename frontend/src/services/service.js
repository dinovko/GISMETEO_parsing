export default class RService {
    static baseUrl = () => 'https://localhost:44314/api/';

    static get (url) {
        return RService.request('GET', url);
    }
    
    static post (url, data) {
        return RService.request('POST', url, data);
    }

    static put (url, data) {
        return RService.request('PUT', url, data);
    }

    static delete (url) {
        return RService.request('DELETE', url);
    }

    static request (method, url, data=null) {
        let body = data;
        let jsonResp = false;
        let isError = false;
        let isBadReq = false;
        let headers = new Headers();
        headers.set('Accept', 'application/json');

        if(data)
        {
            if((typeof data === 'object')) {
                headers.set('Content-Type', 'application/json');
                body = JSON.stringify(data);
            }
            else
            {
                headers.set('Content-Type', 'application/x-www-form-urlencoded');
            }
        }

        return fetch(`${RService.baseUrl()}${url}`, {
            method: method,
            headers: headers,
            body: body
        }).then((resp)=>{
            isBadReq = (resp.status === 400 || resp.status === 404 || resp.status === 403);
            let respContentType = resp.headers.get('content-type');
            if(respContentType && respContentType.indexOf('application/json') !==-1) {
                jsonResp = true;
                return resp.json();
            }            
            else {
                if(respContentType && respContentType.indexOf('application/problem+json') !==-1) {
                    jsonResp = true;
                    return resp.json();
                } else {
                    return resp.text();
                }
            }
        }).then((respContent) => {
            return {
                isError: isBadReq,
                content: isBadReq ? null : respContent
            };
        });
    }
}