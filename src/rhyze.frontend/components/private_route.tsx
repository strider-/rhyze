import ServerCookie from "next-cookies";
import React, { Component } from "react";
import { AppCookies, refreshToken } from "../services/auth_service";
import { AuthToken } from "../services/auth_token";
import { NextPageContext } from "next";

export type AuthProps = {
    auth: AuthToken
}

export function privateRoute(WrappedComponent: any) {
    return class extends Component<AuthProps> {

        static async getInitialProps(ctx: NextPageContext) {
            const authToken = ServerCookie(ctx)[AppCookies.authToken];
            const rToken = ServerCookie(ctx)[AppCookies.refreshToken];
            let auth = new AuthToken(authToken, rToken);

            if (auth.isExpired) {
                const res = await refreshToken(auth);
                if (typeof res === "string") {
                    ctx.res.writeHead(302, {
                        Location: "/login?redirected=true",
                    });
                    ctx.res.end();
                } else {
                    auth = res;
                }
            }

            const initialProps = { auth };
            if (WrappedComponent.getInitialProps) return WrappedComponent.getInitialProps(initialProps);
            return initialProps;
        }

        get auth() {
            // the server pass to the client serializes the token
            // so we have to reinitialize the authToken class
            //
            // @see https://github.com/zeit/next.js/issues/3536
            return new AuthToken(this.props.auth.token, this.props.auth.refreshToken);
        }

        render() {
            return <WrappedComponent auth={this.auth} {...this.props} />;
        }
    };
}