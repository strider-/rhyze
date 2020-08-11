import Head from 'next/head'
import { Navbar, Nav } from 'react-bootstrap'
import { logout } from '../services/auth_service';
import Router from "next/router";

const logOutAndRedirect = async () =>{
    logout();
    await Router.push("/login?loggedOut=true");
}

const Header = () => (
    <>
        <Head>
            <title>Rhyze</title>
            <link rel="icon" href="/favicon.ico" />
        </Head>
        <Navbar sticky="top" collapseOnSelect expand="lg" bg="dark" variant="dark">
            <Navbar.Brand href="/">Rhyze</Navbar.Brand>
            <Navbar.Toggle aria-controls="responsive-navbar-nav" />
            <Nav className="ml-auto">
                <Nav.Link href="#" onClick={async () => await logOutAndRedirect()}>Logout</Nav.Link>
            </Nav>
        </Navbar>
    </>
);

export default Header;