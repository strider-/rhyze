import Head from 'next/head'
import { Navbar, Nav } from 'react-bootstrap'
import { logout } from '../services/auth_service';

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
                <Nav.Link href="#" onClick={() => logout()}>Logout</Nav.Link>
            </Nav>
        </Navbar>
    </>
);

export default Header;